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
    public partial class User_Control_Borrow_Book : UserControl
    {
        MySqlConnection conn;
        MySqlCommand cmd = new MySqlCommand();
        DBServer server = new DBServer();
        MySqlDataAdapter da;
        DataTable dt;
        public User_Control_Borrow_Book()
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

        // This method is to fill dgvIssuedBooks
        private void FillDgvIssuedBooks()
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblissuebooks;";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvIssuedBooks.DataSource = dt;
            conn.Close();
        }
        // This method is to clear textboxes
        private void Clear()
        {
            txtStudent_Id.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtContact.Clear();
            txtBookId.Clear();
            rtxtBookTitle.Clear();
            txtDateIssue.Clear();
            txtReturnDate.Clear();
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            // Display Date
            tssTime.Text = DateTime.Now.ToLongTimeString();
        }

        string sId, fName, lName, contactNum, bookId, bookTitle, dateIssue, returnDate, bookStatus, mbTitle;

        private void btnIssue_Click(object sender, EventArgs e)
        {
            try
            {
                sId = txtStudent_Id.Text;
                fName = txtFirstName.Text;
                lName = txtLastName.Text;
                contactNum = txtContact.Text;
                bookId = Convert.ToInt32(txtBookId.Text).ToString();
                bookTitle = rtxtBookTitle.Text;
                dateIssue = txtDateIssue.Text;
                returnDate = txtReturnDate.Text;
                mbTitle = "University of Rizal System Binangonan Library Management";

                if (string.IsNullOrWhiteSpace(sId) || string.IsNullOrWhiteSpace(fName) || string.IsNullOrWhiteSpace(lName) || string.IsNullOrWhiteSpace(contactNum) || string.IsNullOrWhiteSpace(bookId))
                {
                    MessageBox.Show("Invalid Input. Try again", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();
                }
                else
                {
                    // Check if students have unreturned book
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM tblissuebooks WHERE Student_Id = '" + sId + "';";
                    cmd.ExecuteNonQuery();
                    dt = new DataTable();
                    da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 1)
                    {

                        MessageBox.Show("You still have a book that hasn't been returned. ", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        Clear();
                    }
                    else
                    {
                        // Check if book is available
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = @"SELECT * FROM tblbooks WHERE Book_Id = '" + bookId + "';";
                        cmd.ExecuteNonQuery();
                        dt = new DataTable();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                        conn.Close();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["Book_Status"].ToString() == "NOT AVAILABLE")
                            {
                                MessageBox.Show(dr["Title"].ToString() + " is not available", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                txtBookId.Clear(); 
                                rtxtBookTitle.Clear();
                                txtDateIssue.Clear(); 
                                txtReturnDate.Clear();
                            }
                            else
                            {
                                // Insert into tblIssueboks
                                conn.Open();
                                cmd.Connection = conn;
                                cmd.CommandText = @"INSERT INTO tblissuebooks (Student_Id, First_Name, Last_Name, Contact_No, Book_Id, Book_Title, Date_Issue, Return_Date) VALUES ('" + sId.ToUpper() + "', '" + fName + "', '" + lName + "', '" + contactNum + "', '" + bookId + "', '" + bookTitle + "', '" + dateIssue + "', '" + returnDate + "');";
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                MessageBox.Show("Issued Successfully", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                FillDgvIssuedBooks();

                                // Update availability of a book
                                conn.Open();
                                cmd.Connection = conn;
                                cmd.CommandText = @"UPDATE tblbooks SET Book_Status = 'NOT AVAILABLE' WHERE Book_Id = '" + bookId + "';";
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
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
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void txtStudent_Id_TextChanged(object sender, EventArgs e)
        {
           //
        }

        private void txtBookId_TextChanged(object sender, EventArgs e)
        {
            // 
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Search in dgvIssuedBooks
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblissuebooks WHERE Student_Id LIKE '"+ txtSearch.Text +"%';";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvIssuedBooks.DataSource = dt;
            conn.Close();
        }

        private void User_Control_Borrow_Book_Load(object sender, EventArgs e)
        {
            dgvIssuedBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // Display date
            DateTime date = DateTime.Now;
            tssDate.Text = date.ToString("MMMM dd, dddd, yyyy");
            timerTime.Enabled = true;
            // Fill dgvIssuedBooks
            FillDgvIssuedBooks();

            // For Capslock, Numlock, Scroll Lock, Insert Key
            Application.Idle += CapsLock;
            Application.Idle += NumLock;
            Application.Idle += ScrollLock;
            Application.Idle += Insert;

            dgvIssuedBooks.Columns[0].Visible = false;
        }

        private void btnSearchSId_Click(object sender, EventArgs e)
        {
            // Search Student Id
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblmembers WHERE Student_Id = '" + txtStudent_Id.Text + "';";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    txtFirstName.Text = dr["First_Name"].ToString();
                    txtLastName.Text = dr["Last_Name"].ToString();
                    txtContact.Text = dr["Contact_No"].ToString();
                }
            }
            else
            {
                MessageBox.Show("You need to register first to borrow a book", "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStudent_Id.Clear();
            }
            conn.Close();
        }

        private void btnSearchBId_Click(object sender, EventArgs e)
        {
            // Search Book Id
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblbooks WHERE Book_Id = '" + txtBookId.Text + "';";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DateTime date = DateTime.Now;
                    rtxtBookTitle.Text = dr["Title"].ToString();
                    txtDateIssue.Text = date.ToString("MMMM dd, dddd, yyyy");
                    txtReturnDate.Text = date.AddDays(7).ToString("MMMM dd, dddd, yyyy");
                }
            }
            else
            {
                MessageBox.Show("No book found", "University of Rizal System Binangonan Library Management" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBookId.Clear();
            }
            conn.Close();
        }

        private void txtBookId_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Accept numbers only
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtReturnDate_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
