using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Finals_Computer_Programming
{
    public partial class User_Control_View_Students : UserControl
    {
        MySqlCommand cmd = new MySqlCommand();
        DBServer server = new DBServer();
        int Member_ID = 0;

        public User_Control_View_Students()
        {
            InitializeComponent();
        }

        // This method is to activate tssCaps when capslock key is pressed
        private void CapsLock(object sender, EventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                tssCaps.Enabled = true;
            }
            else
            {
                tssCaps.Enabled = false;
            }
        }
        // This method is to activate tssNum when numlock key is pressed
        private void NumLock(object sender, EventArgs e)
        {
            if (Control.IsKeyLocked(Keys.NumLock))
            {
                tssNum.Enabled = true;
            }
            else
            {
                tssNum.Enabled = false;
            }
        }
        // This method is to activate tssScrl when scroll lock key is pressed 
        private void ScrollLock(object sender, EventArgs e)
        {
            if (Control.IsKeyLocked(Keys.Scroll))
            {
                tssScrl.Enabled = true;
            }
            else
            {
                tssScrl.Enabled = false;
            }
        }
        // This method is to activate tssIns when insert key is pressed
        private void Insert(object sender, EventArgs e)
        {
            if (Control.IsKeyLocked(Keys.Insert))
            {
                tssIns.Text = "OVR";
            }
            else
            {
                tssIns.Text = "INS";
            }
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            // Display Time
            tssTime.Text = DateTime.Now.ToLongTimeString();
        }
        // Save
        private void GridFill()
        {
            using (MySqlConnection mysqlcon = new MySqlConnection(server.connection()))
            {
                mysqlcon.Open();
                MySqlDataAdapter mysqlDA = new MySqlDataAdapter("MembersViewAll", mysqlcon);
                mysqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtblMembers = new DataTable();
                mysqlDA.Fill(dtblMembers);
                dgv_infos.DataSource = dtblMembers;
                dgv_infos.Columns[0].Visible = false;
            }
        }
        // Clear
        private void Clear()
        {
            rtxtAddress.Text = txtCN.Text = txtFirstName.Text = txtLastName.Text = txtSearch.Text = txtStudentId.Text = "";
            cmbx_Department.SelectedIndex = -1;
            Member_ID = 0;
            btnSave.Text = "SAVE";
            btnDelete.Enabled = false;
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             //
        }

        string mbTitle = "University of Rizal System Binangonan Library Management";

        // Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection mysqlcon = new MySqlConnection(server.connection()))
                {
                    mysqlcon.Open();
                    MySqlCommand mysqlCmd = new MySqlCommand("MemberAddOrEdit", mysqlcon);
                    mysqlCmd.CommandType = CommandType.StoredProcedure;
                    mysqlCmd.Parameters.AddWithValue("_Member_Id", Member_ID);
                    mysqlCmd.Parameters.AddWithValue("_Student_Id", txtStudentId.Text.Trim().ToUpper());
                    mysqlCmd.Parameters.AddWithValue("_First_Name", txtFirstName.Text.Trim().ToUpper());
                    mysqlCmd.Parameters.AddWithValue("_Last_Name", txtLastName.Text.Trim().ToUpper());
                    mysqlCmd.Parameters.AddWithValue("_Department", cmbx_Department.SelectedItem);
                    mysqlCmd.Parameters.AddWithValue("_Contact_No", txtCN.Text.Trim());
                    mysqlCmd.Parameters.AddWithValue("_Address", rtxtAddress.Text.Trim().ToUpper());
                    mysqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Submitted Successfully", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    GridFill();
                    mysqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clear();
            }
        }

        private void User_Control_View_Students_Load(object sender, EventArgs e)
        {
            dgv_infos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // Display Date
            DateTime date = DateTime.Now;
            string format = "MMMM dd, dddd, yyyy";
            tssDate.Text = date.ToString(format);
            timerTime.Enabled = true;

            // For Capslock, Numlock, Scroll Lock, Insert Key
            Application.Idle += CapsLock;
            Application.Idle += NumLock;
            Application.Idle += ScrollLock;
            Application.Idle += Insert;

            Clear();
            GridFill();
        }

        // Update
        private void dgv_infos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_infos.CurrentRow.Index != -1)
            {
                txtStudentId.Text = dgv_infos.CurrentRow.Cells[1].Value.ToString();
                cmbx_Department.SelectedItem = dgv_infos.CurrentRow.Cells[4].Value.ToString();
                txtFirstName.Text = dgv_infos.CurrentRow.Cells[2].Value.ToString();
                txtLastName.Text = dgv_infos.CurrentRow.Cells[3].Value.ToString();
                txtCN.Text = dgv_infos.CurrentRow.Cells[5].Value.ToString();
                rtxtAddress.Text = dgv_infos.CurrentRow.Cells[6].Value.ToString();
                Member_ID = Convert.ToInt32(dgv_infos.CurrentRow.Cells[0].Value.ToString());
                btnSave.Text = "UPDATE";
                btnDelete.Enabled = true;
            }     
        }

        // Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection mysqlcon = new MySqlConnection(server.connection()))
                {
                    mysqlcon.Open();
                    MySqlDataAdapter mysqlDA = new MySqlDataAdapter("MemberSearchByValue", mysqlcon);
                    mysqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                    mysqlDA.SelectCommand.Parameters.AddWithValue("_SearchValue", txtSearch.Text);
                    DataTable dtblMembers = new DataTable();
                    mysqlDA.Fill(dtblMembers);
                    dgv_infos.DataSource = dtblMembers;
                    dgv_infos.Columns[0].Visible = false;
                    mysqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clear();
            }
        }

        // Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection mysqlcon = new MySqlConnection(server.connection()))
                {
                    mysqlcon.Open();
                    MySqlCommand mysqlCmd = new MySqlCommand("MembersDeleteByID", mysqlcon);
                    mysqlCmd.CommandType = CommandType.StoredProcedure;
                    mysqlCmd.Parameters.AddWithValue("_Member_Id", Member_ID);
                    mysqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Deleted Successfully", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GridFill();
                    mysqlcon.Close();
                }

                // Insert into tbldelete_students
                using (MySqlConnection deleteConn = new MySqlConnection(server.deletedConnection()))
                {
                    deleteConn.Open();
                    cmd.Connection = deleteConn;
                    cmd.CommandText = @"INSERT INTO tbldeleted_students (Student_Id, Department, First_Name, Last_Name, Contact_No, Address) VALUES ('" + txtStudentId.Text + "', '" + cmbx_Department.SelectedItem + "', '" + txtFirstName.Text + "', '" + txtLastName.Text + "', '" + txtCN.Text + "', '" + rtxtAddress.Text + "')";
                    cmd.ExecuteNonQuery();
                    deleteConn.Close();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clear();
            }
        }

        private void txtCN_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Accept numbers only
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
