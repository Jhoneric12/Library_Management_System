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
    public partial class User_Control_Library : UserControl
    {
        MySqlConnection conn;
        MySqlConnection deleteConn;
        MySqlCommand cmd = new MySqlCommand();
        DBServer server = new DBServer();
        MySqlDataAdapter da;
        DataTable dt;

        public User_Control_Library()
        {
            InitializeComponent();
            conn = new MySqlConnection(server.connection());
            deleteConn = new MySqlConnection(server.deletedConnection());
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
        // Clear
        private void Clear()
        {
            rtxtBookTitle.Clear();
            txtAuthor.Clear();
            txtGenre.Clear();
            cmbx_BookStatus.SelectedIndex = -1;
        }
        // Fill dgvBooks
        private void FillDgvBooks()
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblbooks;";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvBooks.DataSource = dt;
            conn.Close();
        }
        // Fill cmbxGenre
        private void FillGenre()
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT DISTINCT Genre FROM tblbooks;";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                cmbxGenre.Items.Add(dr["Genre"].ToString());
            }
            conn.Close();
        }
        private void timerTime_Tick(object sender, EventArgs e)
        {
            // Display Time
            tssTime.Text = DateTime.Now.ToLongTimeString();
        }

        private void User_Control_Library_Load(object sender, EventArgs e)
        {
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // Display date
            DateTime date = DateTime.Now;
            tssDate.Text = date.ToString("MMMM dd, dddd, yyyy");
            timerTime.Enabled = true;

            // For Capslock, Numlock, Scroll Lock, Insert Key
            Application.Idle += CapsLock;
            Application.Idle += NumLock;
            Application.Idle += ScrollLock;
            Application.Idle += Insert;

            // Fill dgvBooks
            FillDgvBooks();

            // Fill cmbxGenre
            FillGenre();

            cmbxGenre.SelectedIndex = 0;
        }

        string bookTitle, bookAuthor, genre, mbTitle;
        int bookId;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bookTitle = rtxtBookTitle.Text;
                bookAuthor = txtAuthor.Text;
                genre = txtGenre.Text;
                mbTitle = "University of Rizal System Binangonan Library Management";

                if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookAuthor) || string.IsNullOrWhiteSpace(genre) || cmbx_BookStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Input. Try Again", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();
                }
                else
                {
                    // Check if book is already exist
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM tblbooks WHERE Title = '" + bookTitle + "';";
                    cmd.ExecuteNonQuery();
                    dt = new DataTable();
                    da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 1)
                    {
                        MessageBox.Show("This book is already exist in the library", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Clear();
                    }
                    else
                    {
                        // Insert books
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = @"INSERT INTO tblbooks (Title, Author, Genre, Book_Status) VALUES ('" + bookTitle.ToUpper() + "', '" + bookAuthor.ToUpper() + "', '" + genre.ToUpper() + "', '" + cmbx_BookStatus.SelectedItem + "');";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Added Successfully", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                        FillDgvBooks();
                        conn.Close();
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                bookTitle = rtxtBookTitle.Text;
                bookAuthor = txtAuthor.Text;
                genre = txtGenre.Text;
                mbTitle = "University of Rizal System Binangonan Library Management";

                if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookAuthor) || string.IsNullOrWhiteSpace(genre) || cmbx_BookStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Input. Try Again", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();
                }
                else
                {
                    // Update Books
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = @"UPDATE tblbooks SET Title = '" + bookTitle.ToUpper() + "', Author = '" + bookAuthor.ToUpper() + "', Genre = '" + genre.ToUpper() + "', Book_Status = '" + cmbx_BookStatus.SelectedItem + "' WHERE Book_Id = '" + bookId + "'";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Update Successfully", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    FillDgvBooks();
                    conn.Close();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bookTitle = rtxtBookTitle.Text;
                bookAuthor = txtAuthor.Text;
                genre = txtGenre.Text;
                mbTitle = "University of Rizal System Binangonan Library Management";

                if (string.IsNullOrWhiteSpace(bookTitle) || string.IsNullOrWhiteSpace(bookAuthor) || string.IsNullOrWhiteSpace(genre) || cmbx_BookStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Invalid Input. Try Again", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();
                }
                else
                {
                    // Delete from tblbooks
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = @"DELETE FROM tblbooks WHERE Book_Id = '" + bookId + "';";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Deleted Successfully", mbTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillDgvBooks();
                    conn.Close();

                    // Insert into tblDeletedBooks
                    deleteConn.Open();
                    cmd.Connection = deleteConn;
                    cmd.CommandText = @"INSERT INTO tbldeleted_books (Book_Id, Title, Genre, Author) VALUES ('" + bookId + "','" + bookTitle + "', '" + genre + "', '" + bookAuthor + "');";
                    cmd.ExecuteNonQuery();
                    deleteConn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clear();
            }
            finally
            {
                // Close connections
                if (conn.State == ConnectionState.Open) conn.Close();
                if (deleteConn.State == ConnectionState.Open) deleteConn.Close();
            }
        }

        private void dgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Display Selected Rows
            if (e.RowIndex == -1) return;

            DataGridViewRow selectedRow = dgvBooks.Rows[e.RowIndex];

            bookId = Convert.ToInt32(selectedRow.Cells[0].Value.ToString());
            rtxtBookTitle.Text = selectedRow.Cells[1].Value.ToString();
            txtAuthor.Text = selectedRow.Cells[2].Value.ToString();
            txtGenre.Text = selectedRow.Cells[3].Value.ToString();
            cmbx_BookStatus.SelectedItem = selectedRow.Cells[4].Value.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtSearchBookTitle_TextChanged(object sender, EventArgs e)
        {
            // Search
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblbooks WHERE Title LIKE '%" + txtSearchBookTitle.Text + "%';";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvBooks.DataSource = dt;
            conn.Close();
        }

        private void cmbxGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = @"SELECT * FROM tblbooks WHERE Genre = '" + cmbxGenre.SelectedItem + "';";
            cmd.ExecuteNonQuery();
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dgvBooks.DataSource = dt;
            conn.Close();

            if (cmbxGenre.SelectedItem == "ALL") FillDgvBooks();
        } 
    }
}
