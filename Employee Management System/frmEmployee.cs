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

        private void LoadEmployeeData()
        {
            //Data Source = MSI\SQLEXPRESS; Initial Catalog = Employees; Integrated Security = True; Trust Server Certificate = True
            using (SqlConnection conn = new SqlConnection(@"Server=MSI\SQLEXPRESS;Database=Employees;Integrated Security=True;Encrypt=False;"))
            {
                conn.Open();
                string query = "SELECT EmployeeID, Name, Position, Salary FROM Employees";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dtvEmployees.DataSource = dt;
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
        }
    }
}
