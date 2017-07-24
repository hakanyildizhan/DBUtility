using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
using System.Configuration;

namespace DBUtility.Core
{
    public class DBArtifact
    {
        private string _fileName { get; set; }

        public string DatabaseName { get; set; }
        public string ServerName { get; set; }
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(this._fileName)) return this.DatabaseName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".bak";
                else return this._fileName;
            }
            set { this._fileName = value; }
        }

        public Server Server
        {
            get
            {
                if (string.IsNullOrEmpty(this.ServerName)) return null;
                else
                {
                    var builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[this.ServerName].ConnectionString);
                    return new Server(new ServerConnection(builder.DataSource, builder.UserID, builder.Password));
                }
            }
        }

        public FileInfo File
        {
            get
            {
                return new FileInfo(Path.Combine(ConfigurationManager.AppSettings["BackupPath"], this.FileName));
                //else return new FileInfo(this.FileName);
            }
        }
    }
}
