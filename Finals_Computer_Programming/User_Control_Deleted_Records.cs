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
    public partial class User_Control_Deleted_Records : UserControl
    {
        MySqlConnection conn;
        MySqlCommand cmd = new MySqlCommand();
        DBServer server = new DBServer();
        MySqlDataAdapter da;
        DataTable dt;

        public User_Control_Deleted_Records()
        {
            InitializeComponent();
            conn = new MySqlConnection(server.deletedConnection());
        }

        private void User_Control_Deleted_Records_Load(object sender, EventArgs e)
        {
            // Display Date
            DateTime date = DateTime.Now;
            string format = "MMMM dd, dddd, yyyy";
            tssDate.Text = date.ToString(format);
            timerTime.Enabled = true;

            dgvDeletedBooks.Columns[0].Visible = false;
            dgvDeletedStudent.Columns[0].Visible = false;

            // Auto Resize columns
            //dgvDeletedStudent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // For Capslock, Numlock, Scroll Lock, Insert Key
            Application.Idle += CapsLock;
            Application.Idle += NumLock;
            Application.Idle += ScrollLock;
            Application.Idle += Insert;

            // Fill DGV
            FillDgvDeletedStudents();
            FillDgcDeletedBooks();
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
        // Fill dgvDeleteStudents
        private void FillDgvDeletedStudents()
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tbldeleted_students;";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvDeletedStudent.DataSource = dt;
            conn.Close();
        }
        // Fill dgvDeletedBooks
        private void FillDgcDeletedBooks()
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tbldeleted_books;";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvDeletedBooks.DataSource = dt;
            conn.Close();
        }
        private void timerTime_Tick(object sender, EventArgs e)
        {
            // Display Time
            tssTime.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
