using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility.Core
{
    public enum UtilityType
    {
        Backup,
        Restore
    }
    public class UtilityFactory
    {
        public static IUtility Instance(UtilityType type, DBArtifact db)
        {
            switch (type)
            {
                case UtilityType.Backup:
                    return new BackupUtility(db);
                case UtilityType.Restore:
                    return new RestoreUtility(db);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
