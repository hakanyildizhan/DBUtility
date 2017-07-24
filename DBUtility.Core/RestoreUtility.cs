using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace DBUtility.Core
{
    class RestoreUtility : IUtility
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private DBArtifact _db;
        public event Action<int> ProgressChanged;

        public RestoreUtility(DBArtifact db)
        {
            _db = db;
        }

        private void OnProgressChanged(int progress)
        {
            ProgressChanged?.Invoke(progress);
        }

        public Response Task()
        {
            Response response = new Response();
            string dbBakFile = _db.File.FullName;
            string dbtoRestore = _db.DatabaseName;

            if (string.IsNullOrEmpty(dbBakFile) || !File.Exists(dbBakFile))
            {
                response.Status = Response.ResponseMessage.Failed;
                response.Message = "No such .bak file found.";
                return response;
            }

            var smoServer = _db.Server;

            var db = smoServer.Databases[dbtoRestore];

            if (db != null)
            {
                smoServer.KillAllProcesses(dbtoRestore);
                log.Debug("All processes on db killed");
                db.SetOffline();
            }

            string dataPath = ConfigurationManager.AppSettings["DataPath"];
            string dbPath = Path.Combine(dataPath, dbtoRestore + ".mdf");
            string logPath = Path.Combine(dataPath, dbtoRestore + ".ldf");

            var restore = new Restore();
            var deviceItem = new BackupDeviceItem(dbBakFile, DeviceType.File);
            
            restore.ReplaceDatabase = true;
            restore.PercentCompleteNotification = 10;
            restore.PercentComplete += restore_PercentComplete;
            restore.Complete += restore_Complete;

            try
            {
                restore.Devices.Add(deviceItem);
                if (db != null)
                {
                    DataTable dtFileList = restore.ReadFileList(smoServer);
                    string dbLogicalName = dtFileList.Rows[0][0].ToString();
                    string logLogicalName = dtFileList.Rows[1][0].ToString();
                    restore.RelocateFiles.Add(new RelocateFile(dbLogicalName, dbPath));
                    restore.RelocateFiles.Add(new RelocateFile(logLogicalName, logPath));
                }
                else
                {
                    Database smoDatabase = new Database(smoServer, dbtoRestore);
                    FileGroup fg = new FileGroup(smoDatabase, "PRIMARY");
                    smoDatabase.FileGroups.Add(fg);
                    DataFile df = new DataFile(fg, dbtoRestore, dbPath);
                    df.IsPrimaryFile = true;
                    fg.Files.Add(df);
                    
                    LogFile lf = new LogFile(smoDatabase, dbtoRestore + "_log", logPath);
                    smoDatabase.LogFiles.Add(lf);
                    smoDatabase.Create();
                    smoServer.Refresh();
                    smoServer.KillAllProcesses(dbtoRestore);
                    log.Debug("All processes on db killed");
                    smoServer.Databases[dbtoRestore].SetOffline();
                    //restore.DatabaseFiles.Add(dbPath);
                    //restore.DatabaseFiles.Add(logPath);
                    DataTable dtFileList = restore.ReadFileList(smoServer);
                    string dbLogicalName = dtFileList.Rows[0][0].ToString();
                    string logLogicalName = dtFileList.Rows[1][0].ToString();
                    restore.RelocateFiles.Add(new RelocateFile(dbLogicalName, dbPath));
                    restore.RelocateFiles.Add(new RelocateFile(logLogicalName, logPath));
                }
                restore.Database = dbtoRestore;
                restore.FileNumber = 1;
                restore.Action = RestoreActionType.Files;
                restore.SqlRestore(smoServer);
                db = smoServer.Databases[dbtoRestore];
                db.SetOnline();
                smoServer.Refresh();
                db.Refresh();
            }
            catch (Exception ex)
            {
                response.Status = Response.ResponseMessage.Failed;
                response.Message = ex.Message;
                return response;
            }
            
            response.Status = Response.ResponseMessage.Succeeded;
            response.Message = string.Format("{0} file restored to database {1}.", _db.File.Name, dbtoRestore);
            return response;
        }

        private void restore_Complete(object sender, ServerMessageEventArgs e)
        {
            log.Info("Finished");
        }

        private void restore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            OnProgressChanged(e.Percent);
            log.Info("{0}% complete", e.Percent);
        }
    }
}
