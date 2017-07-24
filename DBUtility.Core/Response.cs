using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility.Core
{
    public class Response
    {
        public string Message { get; set; }
        public ResponseMessage Status { get; set; }

        public enum ResponseMessage
        {
            Failed,
            Succeeded
        }
    }
}
