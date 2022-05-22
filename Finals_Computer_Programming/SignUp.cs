using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Finals_Computer_Programming
{
    public partial class SignUp : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd = new MySqlCommand();
        DBServer server = new DBServer();
        MySqlDataReader dr;
        Form1 form1 = new Form1();

        public SignUp()
        {
            InitializeComponent();
            conn = new MySqlConnection(server.connection());
        }

        private void lblLogIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            form1.Show();
        }

        string userName, passWord, firstName, lastName;

        private void SignUp_Load(object sender, EventArgs e)
        {
            cmbxUserType.SelectedIndex = 0;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            userName = txtUsername.Text;
            passWord = txtPassWord.Text;
            firstName = txtFirstName.Text;
            lastName = txtLastName.Text;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(passWord) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || cmbxUserType.SelectedIndex == -1)
            {
                MessageBox.Show("Invalid Input. Try another one", "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clear();
            }
            else
            {
                // Verify if username already exist in database
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT * FROM users_account WHERE User_Name = '" + txtUsername.Text + "';";
                cmd.ExecuteNonQuery();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    MessageBox.Show("Username already exist. Try another one", "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();
                }
                else
                {
                    // Insert into users_account table
                    if (conn.State == ConnectionState.Open) conn.Close();
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = @"INSERT INTO users_account (User_Name, Pass_Word, First_Name, Last_Name, User_Type) VALUES ('" + userName + "', '" + passWord + "', '" + firstName.ToUpper() + "', '" + lastName.ToUpper() + "', '" + cmbxUserType.SelectedItem + "');";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully created. Press Ok to Log In", "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    this.Hide();
                    form1.Show();
                }
                conn.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "University of Rizal System Binangonan Library Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
            else { return; }
        }

        // This methos is to clear textboxes
        private void Clear()
        {
            txtUsername.Clear();
            txtPassWord.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            cmbxUserType.SelectedIndex = -1;
        }
    }
}
