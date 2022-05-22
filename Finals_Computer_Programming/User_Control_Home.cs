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
    public partial class User_Control_Home : UserControl
    {
        MySqlConnection conn;
        MySqlCommand cmd = new MySqlCommand();
        DBServer server = new DBServer();
        public User_Control_Home()
        {
            InitializeComponent();
            conn = new MySqlConnection(server.connection());
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 
        }

        private void User_Control_Home_Load(object sender, EventArgs e)
        {
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

            CountStudents();
            CountBooks();
            CountUnreturnedBooks();
            CountReturnedBooks();
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
        // Count Students
        private void CountStudents()
        {
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT COUNT(*) FROM tblmembers";
            cmd.ExecuteNonQuery();
            int students = Convert.ToInt32(cmd.ExecuteScalar());
            lblStudents.Text = students.ToString();
            conn.Close();
        }
        // Count Books
        private void CountBooks()
        {
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT COUNT(*) FROM tblbooks";
            cmd.ExecuteNonQuery();
            int books = Convert.ToInt32(cmd.ExecuteScalar());
            lblBooks.Text = books.ToString();
            conn.Close();
        }
        // Count unreturned books
        private void CountUnreturnedBooks()
        {
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT COUNT(*) FROM tblissuebooks";
            cmd.ExecuteNonQuery();
            int unreturn = Convert.ToInt32(cmd.ExecuteScalar());
            lblUnreturnedBooks.Text = unreturn.ToString();
            conn.Close();
        }
        // Count returned books
        private void CountReturnedBooks()
        {
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT COUNT(*) FROM tblreturnbooks";
            cmd.ExecuteNonQuery();
            int returned = Convert.ToInt32(cmd.ExecuteScalar());
            lblReturnedBooks.Text = returned.ToString();
            conn.Close();
        }
    }
}
