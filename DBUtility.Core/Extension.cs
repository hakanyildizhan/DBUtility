using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility.Core
{
    public static class Extension
    {
        public static string GetExceptionMessages(this Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\n" + GetExceptionMessages(e.InnerException);
            return msgs;
        }

        public static string GetValue(this Enum enumValue)
        {
            return nameof(enumValue);
        }

        public static EnumType ToEnum<EnumType>(this String enumValue)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), enumValue);
        }
        
    }
}
