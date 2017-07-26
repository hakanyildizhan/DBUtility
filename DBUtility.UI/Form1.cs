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
                ServerName = cmbServer.Text,
                DatabaseName = GetSelectedDatabase(),
                FileName = txtFilename.Text
            };

            if (MessageBox.Show(string.Format("{0} file will be restored with the name {1} on server {2}. Do you want to continue?", db.FileName, db.DatabaseName, db.ServerName), "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IUtility utility = UtilityFactory.Instance(UtilityType.Restore, db);
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (obj, ea) => Restore(utility);
                worker.RunWorkerAsync();
            }
            
        }

        private void Restore(IUtility utility)
        {
            HandleTask(utility);
        }
        private void btnBackup_Click(object sender, EventArgs e)
        {
            DBArtifact db = new DBArtifact()
            {
                ServerName = cmbServer.Text,
                DatabaseName = GetSelectedDatabase()
            };

            IUtility utility = UtilityFactory.Instance(UtilityType.Backup, db);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (obj,ea) => Backup(utility);
            worker.RunWorkerAsync();
        }
        
        private void Backup(IUtility utility)
        {
            HandleTask(utility);
        }

        private void HandleTask(IUtility utility)
        {
            ToggleForm(false);
            utility.ProgressChanged += Utility_ProgressChanged;
            Response res = utility.Task();
            if (MessageBox.Show(res.Message, res.Status.ToString()) == DialogResult.OK)
                SetProgressBar(0);
            ToggleForm(true);
        }
        
        private void Utility_ProgressChanged(int progress)
        {
            this.UpdateProgressBar(progress);
        }

        private void UpdateProgressBar(int progress)
        {
            Action action = () =>
            {
                progressBar1.Step = progress;
                progressBar1.PerformStep();
            };
            progressBar1.Invoke(action);
        }

        private void SetProgressBar(int value)
        {
            Action action = () => progressBar1.Value = value;
            progressBar1.Invoke(action);
        }

        private void chkNewDb_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewDb.Checked)
            {
                cmbDbName.Enabled = false;
                cmbDbName.Visible = false;
                cmbDbName.Text = string.Empty;
                txtDbName.Enabled = true;
                txtDbName.Visible = true;
            }
            else
            {
                cmbDbName.Enabled = true;
                cmbDbName.Visible = true;
                txtDbName.Enabled = false;
                txtDbName.Visible = false;
                txtDbName.Text = string.Empty;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cmbServer.SelectedIndexChanged -= new System.EventHandler(this.cmbServer_SelectedIndexChanged);
            cmbServer.DataSource = ServerManager.GetServers();
            cmbServer.SelectedItem = null;
            this.cmbServer.SelectedIndexChanged += new System.EventHandler(this.cmbServer_SelectedIndexChanged);
            btnBackup.Enabled = false;
            btnRestore.Enabled = false;
        }

        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbServer.Text))
            {
                List<string> dbs = ServerManager.GetDatabases(cmbServer.Text);
                cmbDbName.DataSource = dbs;
            }
        }

        private void txtDbName_TextChanged(object sender, EventArgs e)
        {
            txtFilename.Text = txtDbName.Text;
            if (!string.IsNullOrEmpty(txtDbName.Text)) btnRestore.Enabled = true;
        }

        private void cmbDbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilename.Text = cmbDbName.Text;
            if (!string.IsNullOrEmpty(cmbDbName.Text)) btnBackup.Enabled = true;
        }

        private string GetSelectedDatabase()
        {
            string selectedDB = string.Empty;
            if (cmbDbName.Enabled) selectedDB = cmbDbName.Text;
            else selectedDB = txtDbName.Text;

            if (!string.IsNullOrEmpty(selectedDB)) return selectedDB;
            else throw new Exception("No DB selected.");
        }

        private void ToggleForm(bool enable)
        {
            List<Control> controls = this.Controls.Cast<Control>().Where(c => !(c is ProgressBar) && c.Visible).ToList();
            foreach (Control control in controls)
            {
                Action action = () => control.Enabled = enable;
                control.Invoke(action);
            }
        }
    }
}
