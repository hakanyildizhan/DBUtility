using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBUtility.Core;

namespace DBUtility.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            DBArtifact db = new DBArtifact()
            {
                ServerName = "SQL2014",
                DatabaseName = "SQLTEST"
            };

            IUtility utility = UtilityFactory.Instance(UtilityType.Restore, db);
            Response res = utility.Task();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            DBArtifact db = new DBArtifact()
            {
                ServerName = "SQL2014",
                DatabaseName = "SQLTEST"
            };

            IUtility utility = UtilityFactory.Instance(UtilityType.Backup, db);
            Response res = utility.Task();
        }
    }
}
