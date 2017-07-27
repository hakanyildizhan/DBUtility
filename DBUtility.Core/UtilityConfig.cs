using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility.Core
{
    public static class UtilityConfig
    {
        private static NameValueCollection _machineConfig;
        private static ConnectionStringSettingsCollection _connectionStrings;

        static UtilityConfig()
        {
            _machineConfig = (NameValueCollection)ConfigurationManager.GetSection("MachineConfig");
            _connectionStrings = ConfigurationManager.ConnectionStrings;
        }

        public static string GetMachineConfigValue(string key)
        {
            return _machineConfig.Get(key);
        }

        public static string GetConnectionString(string instanceName)
        {
            return _connectionStrings[instanceName].ConnectionString;
        }
    }
}
