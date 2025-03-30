using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee_Management_System
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmRegister frmRegister = new frmRegister();
            frmRegister.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            using (SqlConnection connection = new SqlConnection("Server=MSI\\SQLEXPRESS;Database=Users;Integrated Security=True;Encrypt=False;"))
            {
                connection.Open();

                // Query user's password and role
                string query = "SELECT PasswordHash, Role FROM Users WHERE Username = @username";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // If the user is found
                        {
                            byte[] storedPasswordHash = (byte[])reader["PasswordHash"];
                            string role = reader["Role"].ToString(); // Get the user's role

                            // Hash the password entered by the user with SHA-256
                            using (SHA256 sha256 = SHA256.Create())
                            {
                                byte[] enteredPasswordHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                                // If passwords match
                                if (storedPasswordHash.SequenceEqual(enteredPasswordHash))
                                {
                                    MessageBox.Show($"Login successful! Welcome, {username}");

                                    // When entering the main form, also pass the role
                                    frmMain main = new frmMain(username, role);
                                    main.Show();
                                    this.Hide(); // Close login screen
                                }
                                else
                                {
                                    MessageBox.Show("Incorrect password!");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("User not found!");
                        }
                    }
                }
            }
        }

        
    }
}
