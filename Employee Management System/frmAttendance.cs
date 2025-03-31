using DocumentFormat.OpenXml.Office.CustomUI;
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


namespace Employee_Management_System
{
    public partial class frmAttendance : Form
    {
        private string connectionString = @"Server=MSI\SQLEXPRESS;Database=Employees;Integrated Security=True;Encrypt=False;";

        public frmAttendance()
        {
            InitializeComponent();
            LoadAttendance();
        }

        private void LoadAttendance()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT 
                e.EmployeeID, 
                e.Name, 
                a.Status 
            FROM Employees e
            LEFT JOIN Attendance a ON e.EmployeeID = a.EmployeeID 
                AND a.AttendanceDate = CAST(GETDATE() AS DATE)"; // Get today's records

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dtvAttendance.DataSource = dt;
            }
        }

       

        private void btnManageEmployees_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmEmployee employee = new frmEmployee(frmMain.currentRole); 
            employee.ShowDialog();
        }

        private void btnMarkAttendance_Click_1(object sender, EventArgs e)
        {
            if (dtvAttendance.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an employee.");
                return;
            }

            int employeeID = Convert.ToInt32(dtvAttendance.SelectedRows[0].Cells["EmployeeID"].Value);
            string status = cmbStatus.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Please select attendance status.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            MERGE INTO Attendance AS target
            USING (SELECT @EmployeeID AS EmployeeID, CAST(GETDATE() AS DATE) AS AttendanceDate) AS source
            ON target.EmployeeID = source.EmployeeID AND target.AttendanceDate = source.AttendanceDate
            WHEN MATCHED THEN 
                UPDATE SET Status = @Status
            WHEN NOT MATCHED THEN 
                INSERT (EmployeeID, AttendanceDate, Status) VALUES (@EmployeeID, CAST(GETDATE() AS DATE), @Status);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Attendance marked successfully.");
            LoadAttendance(); // DataGridView is updated by reloading
        }


        
    }
}
