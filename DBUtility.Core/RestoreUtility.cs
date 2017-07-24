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

        public RestoreUtility(DBArtifact db)
        {
            _db = db;
        }

        public Response Task()
        {
            Response response = new Response();
            string dbBakFile = _db.File.FullName;
            string dbtoRestore = _db.DatabaseName;

            if (string.IsNullOrEmpty(dbBakFile))
            {
                response.Status = Response.ResponseMessage.Failed;
                response.Message = "No .bak file found in " + dbtoRestore;
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

            //restore.DatabaseFiles.Add(dbPath);
            //restore.DatabaseFiles.Add(logPath);
            
            restore.ReplaceDatabase = true;
            restore.PercentCompleteNotification = 10;
            restore.PercentComplete += restore_PercentComplete;
            restore.Complete += restore_Complete;

            try
            {
                restore.Devices.Add(deviceItem);
                DataTable dtFileList = restore.ReadFileList(smoServer);
                string dbLogicalName = dtFileList.Rows[0][0].ToString();
                string logLogicalName = dtFileList.Rows[1][0].ToString();
                restore.RelocateFiles.Add(new RelocateFile(dbLogicalName, dbPath));
                restore.RelocateFiles.Add(new RelocateFile(logLogicalName, logPath));
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
            return response;
        }

        private void restore_Complete(object sender, ServerMessageEventArgs e)
        {
            log.Info("Finished");
        }

        private void restore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            log.Info("{0}% complete", e.Percent);
        }
    }
}
