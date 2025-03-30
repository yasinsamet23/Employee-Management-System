using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee_Management_System
{
    public partial class frmMain : Form
    {
        private string currentUser;
        private string currentRole;
        public frmMain(string username, string role)
        {
            InitializeComponent();
            currentUser = username;
            currentRole = role;
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {currentUser}"; // Show Username

            btnManageEmployees.Enabled = true;
        }

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            // Open frmEmployee by authorization
            frmEmployee manageEmployees = new frmEmployee(currentRole);
            manageEmployees.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin login = new frmLogin();
            login.Show();
        }
    }
}
