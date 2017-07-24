using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;

namespace DBUtility.Core
{
    public class ServerManager
    {
        public static List<string> GetServers()
        {
            //DataTable dt = SmoApplication.EnumAvailableSqlServers(ConfigurationManager.AppSettings["DBServerName"]);

            //foreach (DataRow dr in dt.Rows)
            //{
            //    string name = dr["Name"].ToString();
            //    string server = dr["Server"].ToString();
            //    string instance = dr["Instance"].ToString();
            //}

            return new List<string>(){ "SQL2008", "SQL2012", "SQL2014" };
        }

        public static List<string> GetDatabases(string instanceName)
        {
            var builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[instanceName].ConnectionString);
            Server srv = new Server (new ServerConnection(builder.DataSource, builder.UserID, builder.Password));
            return srv.Databases.Cast<Database>().Where(d => !d.IsSystemObject).Select(d => d.Name).ToList();
        }
    }
}
