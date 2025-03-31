using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ClosedXML.Excel;
using System.Net.Mail;
using System.Net;

namespace Employee_Management_System
{
    public partial class frmEmployee : Form
    {
        private string currentRole;
        public frmEmployee(string role)
        {
            InitializeComponent();
            currentRole = role;
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            LoadEmployeeData();


            if (currentRole != "Admin")
            {
                dtvEmployees.ReadOnly = true; // User can only see data, cannot change it
            }
        }

        private void LoadEmployeeData(string filter = "")
        {
            //Data Source = MSI\SQLEXPRESS; Initial Catalog = Employees; Integrated Security = True; Trust Server Certificate = True
            using (SqlConnection conn = new SqlConnection(@"Server=MSI\SQLEXPRESS;Database=Employees;Integrated Security=True;Encrypt=False;"))
            {
                conn.Open();
                string query = "SELECT EmployeeID, Name, Position, Salary FROM Employees";
                // If there is a filter, adjust the query accordingly
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query += " WHERE Name LIKE @filter OR Position LIKE @filter";
                }
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@filter", "%" + filter + "%"); // Filter data received from user

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dtvEmployees.DataSource = dt;
            }
        }

        // CSV Export
        private void ExportToCSV()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV File (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("EmployeeID,Name,Position,Salary");

                foreach (DataGridViewRow row in dtvEmployees.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        sb.AppendLine($"{row.Cells["EmployeeID"].Value},{row.Cells["Name"].Value},{row.Cells["Position"].Value},{row.Cells["Salary"].Value}");
                    }
                }
                
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());
                MessageBox.Show("Data successfully exported to CSV.");
            }
        }

        public void ExportToExcel()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Employees");

                    // Add headers
                    worksheet.Cell(1, 1).Value = "EmployeeID";
                    worksheet.Cell(1, 2).Value = "Name";
                    worksheet.Cell(1, 3).Value = "Position";
                    worksheet.Cell(1, 4).Value = "Salary";

                    // Add data rows
                    int row = 2;
                    foreach (DataGridViewRow dataRow in dtvEmployees.Rows)
                    {
                        if (!dataRow.IsNewRow)
                        {


                            // Convert data in cells to appropriate type (by checking if they are empty)
                            worksheet.Cell(row, 1).Value = dataRow.Cells["EmployeeID"].Value?.ToString() ?? ""; // ID (int may be)
                            worksheet.Cell(row, 2).Value = dataRow.Cells["Name"].Value?.ToString() ?? "";      // Name (string)
                            worksheet.Cell(row, 3).Value = dataRow.Cells["Position"].Value?.ToString() ?? "";  // Position (string)
                            worksheet.Cell(row, 4).Value = dataRow.Cells["Salary"].Value?.ToString() ?? "";    // Salary (can be decimal or float)
                            row++;

                        }
                    }

                    // Dosyayı kaydedin
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Data successfully exported to Excel.");
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (currentRole != "Admin")
            {
                MessageBox.Show("You do not have permission!");
                return;
            }

            string name = txtName.Text.Trim();
            string position = txtPosition.Text.Trim();
            float salary;

            if (!float.TryParse(txtSalary.Text, out salary))
            {
                MessageBox.Show("Enter a valid salary.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(@"Server=MSI\SQLEXPRESS;Database=Employees;Integrated Security=True;Encrypt=False;"))
            {
                conn.Open();
                string query = "INSERT INTO Employees (Name, Position, Salary) VALUES (@name, @position, @salary)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@position", position);
                    cmd.Parameters.AddWithValue("@salary", salary);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("The employee was added successfully.");
            LoadEmployeeData();
            

            SendEmailNotification(
        "New Employee Added",
        $"A new employee has been added: {name} ({position}).",
        "admin@yourcompany.com" // **📌 WRITE THE RECEIVER'S E-MAIL ADDRESS HERE**
    );
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (currentRole != "Admin")
            {
                MessageBox.Show("You do not have permission!");
                return;
            }

            if (dtvEmployees.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an employee to edit.");
                return;
            }

            int employeeID = Convert.ToInt32(dtvEmployees.SelectedRows[0].Cells["EmployeeID"].Value);
            string name = txtName.Text.Trim();
            string position = txtPosition.Text.Trim();
            float salary;

            if (!float.TryParse(txtSalary.Text, out salary))
            {
                MessageBox.Show("Enter a valid salary.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(@"Server=MSI\SQLEXPRESS;Database=Employees;Integrated Security=True;Encrypt=False;"))
            {
                conn.Open();
                string query = "UPDATE Employees SET Name = @name, Position = @position, Salary = @salary WHERE EmployeeID = @employeeID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@position", position);
                    cmd.Parameters.AddWithValue("@salary", salary);
                    cmd.Parameters.AddWithValue("@employeeID", employeeID);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Employee information has been updated.");
            LoadEmployeeData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentRole != "Admin")
            {
                MessageBox.Show("You do not have permission!");
                return;
            }

            if (dtvEmployees.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an employee to delete.");
                return;
            }

            int employeeID = Convert.ToInt32(dtvEmployees.SelectedRows[0].Cells["EmployeeID"].Value);

            using (SqlConnection conn = new SqlConnection(@"Server=MSI\SQLEXPRESS;Database=Employees;Integrated Security=True;Encrypt=False;"))
            {
                conn.Open();
                string query = "DELETE FROM Employees WHERE EmployeeID = @employeeID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@employeeID", employeeID);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("The employee was deleted successfully.");
            LoadEmployeeData();

            SendEmailNotification(
        "Employee Removed",
        $"Employee {employeeID} has been removed from the system.",
        "admin@yourcompany.com" // **📌 WRITE THE RECEIVER'S E-MAIL ADDRESS HERE**
    );
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            LoadEmployeeData(searchText); // Filter as user types
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            ExportToCSV(); // Export to CSV
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel(); // Export to Excel
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            if (currentRole != "Admin")
            {
                MessageBox.Show("You do not have permission!");
                return;
            }
            this.Hide();
            frmAttendance attendance = new frmAttendance();
            attendance.Show();
        }

        private void SendEmailNotification(string subject, string body, string recipientEmail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("Sender Email Adress "); // **📌 WRITE THE SENDER'S E-MAIL HERE**
                mail.To.Add("Receiver Email Adress"); // **📌 WE PASS THE RECEIVER HERE AS A PARAMETER**
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true; // To send email in HTML format

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); // **📌 Gmail SMTP**
                smtp.Credentials = new NetworkCredential("Sender Email Adress", "Sender Aplication Password");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                MessageBox.Show("Email notification sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }
        }
    }
}