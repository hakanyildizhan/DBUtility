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
    public class BackupUtility : IUtility
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private DBArtifact _db;

        public BackupUtility(DBArtifact db)
        {
            _db = db;
        }

        public Response Task()
        {
            Response response = new Response();
            string dbBakFile = _db.File.FullName;
            string dbtoBackup = _db.DatabaseName;
            
            var smoServer = _db.Server;

            var db = smoServer.Databases[dbtoBackup];

            if (db == null)
            {
                response.Status = Response.ResponseMessage.Failed;
                response.Message = "Target DB not found.";
                return response;
            }

            var backup = new Backup();
            var deviceItem = new BackupDeviceItem(dbBakFile, DeviceType.File);

            backup.PercentCompleteNotification = 10;
            backup.PercentComplete += backup_PercentComplete;
            backup.Complete += backup_Complete;

            try
            {
                backup.Initialize = true;
                backup.Devices.Add(deviceItem);
                backup.Database = dbtoBackup;
                backup.Action = BackupActionType.Database;
                backup.CompressionOption = BackupCompressionOptions.On;
                backup.Incremental = false;
                backup.SqlBackup(smoServer);
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

        private void backup_Complete(object sender, ServerMessageEventArgs e)
        {
            log.Info("Finished.");
        }

        private void backup_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            log.Info("{0}% complete", e.Percent);
        }
    }
}
