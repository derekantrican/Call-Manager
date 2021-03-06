﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Call_Manager
{
    public partial class MasterView : Form
    {
        public MasterView()
        {
            InitializeComponent();

            pictureBoxLogo.Click += new EventHandler(adminMode);

            this.FormClosing += MasterView_FormClosing;

            string sqlServer = Resources.UserSettings.Default.Server;
            string sqlInstance = Resources.UserSettings.Default.Instance;

            string dataconnection = ("Data Source = " + sqlServer + "\\" + sqlInstance + "; Initial Catalog = 3CXSupportDBase; Persist Security Info = True; User ID = AE; Password = ne$t123");

            SqlConnection conn = new SqlConnection(dataconnection);

            using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) from Engineers where AE like @username", conn))
            {
                try
                {
                    conn.Open();
                }
                catch
                {
                    DialogResult result = MessageBox.Show("Couldn't connect to database","Failed to connect",MessageBoxButtons.OK,MessageBoxIcon.Error);

                    if (result == DialogResult.OK || result == DialogResult.Cancel)
                    {
                        this.Close();
                        return;
                    }
                }

                sqlCommand.Parameters.AddWithValue("@username", Environment.UserName);
                int userCount = (int)sqlCommand.ExecuteScalar();

                if (userCount < 1)
                {
                    MessageBox.Show("\"" + Environment.UserName + "\" is not in the system. Please talk to Mike Fink");
                    labelOperatorTab.Enabled = false;
                    labelOperatorTab.BackColor = System.Drawing.Color.LightSteelBlue;
                    labelTierTab.Enabled = false;
                    labelTierTab.BackColor = System.Drawing.Color.PaleGoldenrod;
                    
                }
            }
            conn.Close();
        }

        private void MasterView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Clock.Text = DateTime.Now.ToString("%h:mm tt", CultureInfo.InvariantCulture);
        }

        private void labelOperatorTab_Click(object sender, EventArgs e)
        {
            labelSelectViewPrompt.Visible = false;

            if (this.ActiveMdiChild != null && this.ActiveMdiChild.Text.ToString() == "Tier View")
            {
                this.ActiveMdiChild.Close();
            }         

            OperatorView operatorMDIChild = new OperatorView();
            // Set the Parent Form of the Child window.
            operatorMDIChild.MdiParent = this;
            // Display the new form.
            operatorMDIChild.Show();
            operatorMDIChild.Location = new Point(12, 45);

            labelOperatorTab.BackColor = System.Drawing.Color.LightSteelBlue;
            labelTierTab.BackColor = System.Drawing.Color.Yellow;
            labelOperatorTab.Enabled = false;
            labelTierTab.Enabled = true;
        }

        private void labelTierTab_Click(object sender, EventArgs e)
        {
            labelSelectViewPrompt.Visible = false;

            if (this.ActiveMdiChild != null && this.ActiveMdiChild.Text.ToString() == "Operator View")
            {
                this.ActiveMdiChild.Close();
            }

            TierView tierMDIChild = new TierView();
            // Set the Parent Form of the Child window.
            tierMDIChild.MdiParent = this;
            // Display the new form.
            tierMDIChild.Show();
            tierMDIChild.Location = new Point(12, 45);

            labelTierTab.BackColor = System.Drawing.Color.PaleGoldenrod;
            labelOperatorTab.BackColor = System.Drawing.Color.Blue;
            labelTierTab.Enabled = false;
            labelOperatorTab.Enabled = true;
        }

        private void adminMode(object sender, EventArgs e)
        {
            if (Form.ModifierKeys == Keys.Control)
            {
                AdminMode admin = new AdminMode();
                admin.Show();
            }
        }

        private void MasterView_Load(object sender, EventArgs e)
        {

        }

        private void pictureBoxSettings_Click(object sender, EventArgs e)
        {
            Forms.Config config = new Forms.Config();
            config.Show();
        }
    }
}
