using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DBUtility.WebUI.Models
{
    public class DBActionMeta
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionID { get; set; }
        public ActionType ActionType { get; set; }
        public int UserID { get; set; }

        public virtual ICollection<DBActionDetail> Details { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

    public class DBActionDetail
    {
        [Key, Column(Order = 0)]
        public int ActionID { get; set; }
        [Key, Column(Order = 1)]
        public int Revision { get; set; }
        public int? CurrentProgress { get; set; }
        public ActionResult ActionResult { get; set; }
        public string Detail { get; set; }

        [ForeignKey("ActionID")]
        public virtual DBActionMeta Meta { get; set; }
    }
}