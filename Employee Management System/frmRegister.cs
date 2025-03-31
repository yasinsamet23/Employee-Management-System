using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Employee_Management_System
{
    public partial class frmRegister : Form
    {
        //private string connectionString =@"Data Source=MSI\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;Trust Server Certificate=True";
        private string connectionString = @"Server=MSI\SQLEXPRESS;Database=Users;Integrated Security=True;Encrypt=False;";

        public frmRegister()
        {
            InitializeComponent();

        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Username and password cannot be blank
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be blank", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Passwords must match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Let's hash the password
                    byte[] passwordHash = HashPassword(password);

                    // SQL INSERT query
                    string query = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, 'User')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);  // We save byte array directly instead of Base64

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // Close the form when registration is successful
                            frmLogin login = new frmLogin();
                            login.Show();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static byte[] HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin login = new frmLogin();
            login.Show();

        }


    }


}