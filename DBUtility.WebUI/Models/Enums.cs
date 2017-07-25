using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBUtility.WebUI.Models
{
    public enum ActionType
    {
        Backup,
        Restore
    }

    public enum ActionResult
    {
        Failed,
        Succeeded
    }
}