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
    public partial class User_Control_Return_Books : UserControl
    {
        MySqlConnection conn;
        MySqlCommand cmd = new MySqlCommand();
        DBServer server = new DBServer();
        MySqlDataAdapter da;
        DataTable dt;
        public User_Control_Return_Books()
        {
            InitializeComponent();
            conn = new MySqlConnection(server.connection());
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

        private void timerTime_Tick_1(object sender, EventArgs e)
        {
            // Display Time
            tssTime.Text = DateTime.Now.ToLongTimeString();
        }
        // This method is to fill dgvReturnedBooks
        private void FillDgvReturnedBooks()
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblreturnbooks;";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvReturnedBooks.DataSource = dt;
            conn.Close();
        }
        // This method is to clear textboxes
        private void Clear()
        {
            txtStudentId.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtBookId.Clear();
            rtxtBookTitle.Clear();
            txtDateIssued.Clear();
        }

        string sId, fName, lName, bookId, bookTitle, dateIssued, dateReturned, mbTitle;
        int issueId, result;
        DateTime issueDate, returnDate;

        private void btnReturnBooks_Click(object sender, EventArgs e)
        {
            try
            {
                sId = txtStudentId.Text;
                fName = txtFirstName.Text;
                lName = txtLastName.Text;
                bookId = Convert.ToInt32(txtBookId.Text).ToString();
                bookTitle = rtxtBookTitle.Text;
                dateIssued = txtDateIssued.Text;
                dateReturned = DateTime.Now.ToString("MMMM dd, dddd, yyyy");
                mbTitle = "University of Rizal System Binangonan Library Management";

                if (string.IsNullOrWhiteSpace(sId) || string.IsNullOrWhiteSpace(fName) || string.IsNullOrWhiteSpace(lName) || string.IsNullOrWhiteSpace(bookId) || string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(dateIssued))
                {
                    MessageBox.Show("Invalid Input. Try again", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Clear();
                }
                else
                {
                    // Set Fine
                    issueDate = Convert.ToDateTime(dateIssued);
                    returnDate = Convert.ToDateTime(dateReturned);
                    result = (returnDate - issueDate).Days;
                    if (result > 7)
                    {
                        double fine = 150;
                        if (conn.State == ConnectionState.Open) conn.Close();
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = @"INSERT INTO tblreturnbooks (Student_Id, First_Name, Last_Name, Issue_Id, Book_Id, Book_Title, Date_Issued, Returned_Date, Fine) VALUES ('" + sId + "', '" + fName + "', '" + lName + "', '" + issueId + "', '" + bookId + "','" + bookTitle + "', '" + dateIssued + "', '" + dateReturned + "', '" + fine + "');";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("You returned this book late. Your fine is P150.00. Press Ok   to continue ", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Clear();
                        FillDgvReturnedBooks();
                        conn.Close();

                        // Delete from Issuebbooks
                        if (conn.State == ConnectionState.Open) conn.Close();
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = @"DELETE FROM tblissuebooks WHERE Issue_Id = '" + issueId + "'";
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        // Update Book_Status
                        if (conn.State == ConnectionState.Open) conn.Close();
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = @"UPDATE tblbooks SET Book_Status = 'AVAILABLE' WHERE Book_Id = '" + bookId + "'";
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    else
                    {
                        // Check if student have no issued book
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = @"SELECT * FROM tblissuebooks WHERE Student_Id = '" + sId + "';";
                        cmd.ExecuteNonQuery();
                        dt = new DataTable();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Data found", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Clear();
                        }
                        else
                        {
                            // Insert into tblreturnbooks
                            if (conn.State == ConnectionState.Open) conn.Close();
                            conn.Open();
                            cmd.Connection = conn;
                            cmd.CommandText = @"INSERT INTO tblreturnbooks (Student_Id, First_Name, Last_Name, Issue_Id, Book_Id, Book_Title, Date_Issued, Returned_Date) VALUES ('" + sId + "', '" + fName + "', '" + lName + "', '" + issueId + "', '" + bookId + "','" + bookTitle + "', '" + dateIssued + "', '" + dateReturned + "');";
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Returned Successfully. Thank you!", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Clear();
                            FillDgvReturnedBooks();
                            conn.Close();

                            // Delete from Issuebbooks
                            if (conn.State == ConnectionState.Open) conn.Close();
                            conn.Open();
                            cmd.Connection = conn;
                            cmd.CommandText = @"DELETE FROM tblissuebooks WHERE Issue_Id = '" + issueId + "'";
                            cmd.ExecuteNonQuery();
                            conn.Close();

                            // Update Book_Status
                            if (conn.State == ConnectionState.Open) conn.Close();
                            conn.Open();
                            cmd.Connection = conn;
                            cmd.CommandText = @"UPDATE tblbooks SET Book_Status = 'AVAILABLE' WHERE Book_Id = '" + bookId + "'";
                            cmd.ExecuteNonQuery();
                            conn.Close();
                       }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clear();
            }
            finally
            {
                // Close connection
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void txtStudentId_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void User_Control_Return_Books_Load(object sender, EventArgs e)
        {
            dgvReturnedBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // Display Date
            DateTime date = DateTime.Now;
            tssDate.Text = date.ToString("MMMM dd, dddd, yyyy");
            timerTime.Enabled = true;
            // Fill dgvReturnedBooks
            FillDgvReturnedBooks();

            // For Capslock, Numlock, Scroll Lock, Insert Key
            Application.Idle += CapsLock;
            Application.Idle += NumLock;
            Application.Idle += ScrollLock;
            Application.Idle += Insert;

            dgvReturnedBooks.Columns[0].Visible = false;
            dgvReturnedBooks.Columns[4].Visible = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Search
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblreturnbooks WHERE Student_Id LIKE '%" + txtSearch.Text + "%';";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvReturnedBooks.DataSource = dt;
            conn.Close();
        }

        private void btnSearchSId_Click(object sender, EventArgs e)
        {
            // Search Student Id From tblissuebooks
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblissuebooks WHERE Student_Id = '" + txtStudentId.Text + "'";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    txtFirstName.Text = dr["First_Name"].ToString();
                    txtLastName.Text = dr["Last_Name"].ToString();
                    rtxtBookTitle.Text = dr["Book_Title"].ToString();
                    issueId = Convert.ToInt32(dr["Issue_Id"].ToString());
                    txtBookId.Text = dr["Book_Id"].ToString();
                    txtDateIssued.Text = dr["Date_Issue"].ToString();
                }
            }
            else
            {
                MessageBox.Show("No data found", "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Clear();
            }
            conn.Close();
        }
    }
}
