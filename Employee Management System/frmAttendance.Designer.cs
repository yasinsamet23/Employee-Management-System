namespace Employee_Management_System
{
    partial class frmAttendance
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtvAttendance = new System.Windows.Forms.DataGridView();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.btnMarkAttendance = new System.Windows.Forms.Button();
            this.btnManageEmployees = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtvAttendance)).BeginInit();
            this.SuspendLayout();
            // 
            // dtvAttendance
            // 
            this.dtvAttendance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtvAttendance.Location = new System.Drawing.Point(183, 206);
            this.dtvAttendance.Name = "dtvAttendance";
            this.dtvAttendance.RowHeadersWidth = 51;
            this.dtvAttendance.RowTemplate.Height = 24;
            this.dtvAttendance.Size = new System.Drawing.Size(482, 211);
            this.dtvAttendance.TabIndex = 0;
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "Present",
            "Absent",
            "Late"});
            this.cmbStatus.Location = new System.Drawing.Point(215, 25);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(121, 24);
            this.cmbStatus.TabIndex = 1;
            // 
            // btnMarkAttendance
            // 
            this.btnMarkAttendance.Location = new System.Drawing.Point(183, 115);
            this.btnMarkAttendance.Name = "btnMarkAttendance";
            this.btnMarkAttendance.Size = new System.Drawing.Size(153, 34);
            this.btnMarkAttendance.TabIndex = 2;
            this.btnMarkAttendance.Text = "Mark Attendance";
            this.btnMarkAttendance.UseVisualStyleBackColor = true;
            this.btnMarkAttendance.Click += new System.EventHandler(this.btnMarkAttendance_Click_1);
            // 
            // btnManageEmployees
            // 
            this.btnManageEmployees.Location = new System.Drawing.Point(373, 115);
            this.btnManageEmployees.Name = "btnManageEmployees";
            this.btnManageEmployees.Size = new System.Drawing.Size(171, 34);
            this.btnManageEmployees.TabIndex = 3;
            this.btnManageEmployees.Text = "Manage Employees";
            this.btnManageEmployees.UseVisualStyleBackColor = true;
            this.btnManageEmployees.Click += new System.EventHandler(this.btnManageEmployees_Click);
            // 
            // frmAttendance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnManageEmployees);
            this.Controls.Add(this.btnMarkAttendance);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.dtvAttendance);
            this.Name = "frmAttendance";
            this.Text = "frmAttendance";
            ((System.ComponentModel.ISupportInitialize)(this.dtvAttendance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtvAttendance;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Button btnMarkAttendance;
        private System.Windows.Forms.Button btnManageEmployees;
    }
}