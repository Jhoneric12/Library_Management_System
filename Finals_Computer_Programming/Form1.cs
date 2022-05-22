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
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd = new MySqlCommand();
        DBServer server = new DBServer();
        MySqlDataAdapter da;
        DataTable dt;

        public Form1()
        {
            InitializeComponent();
            conn = new MySqlConnection(server.connection());
        }

        private void lblSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Go to SignUp form
            this.Hide();
            SignUp signup = new SignUp();
            signup.Show();
        }

        string userName, passWord;

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            userName = txtUsername.Text;
            passWord = txtPassWord.Text;

            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(passWord))
            {
                MessageBox.Show("Invalid Input. Try again", "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clear();
            }
            else
            {
                // Verify Username and Password
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT * FROM users_account WHERE BINARY User_Name = '" + userName + "' AND BINARY Pass_Word = '" + passWord + "';";
                cmd.ExecuteNonQuery();
                dt = new DataTable();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                int rows = Convert.ToInt32(dt.Rows.Count.ToString());
                if (rows == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string name, lName, userType;
                        name = dr["First_Name"].ToString();
                        lName = dr["Last_Name"].ToString();
                        userType = dr["User_Type"].ToString();
                        MessageBox.Show("Welcome " + dr["First_Name"].ToString() + " " + dr["Last_Name"].ToString(), "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HomePage homepage = new HomePage(name, lName, userType);
                        this.Hide();
                        homepage.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Username and Password. Try again", "University of Rizal System Binangonan Library Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();
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

        private void chck_BoxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chck_BoxShowPass.Checked)
            {
                txtPassWord.UseSystemPasswordChar = false; // Show password
                chck_BoxShowPass.Text = "Hide Password";
            }
            else
            {
                txtPassWord.UseSystemPasswordChar = true; // Hide password
                chck_BoxShowPass.Text = "Show Password";
            }
        }

        // This method is to clear textboxes
        private void Clear()
        {
            txtUsername.Clear();
            txtPassWord.Clear();
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //
        }

        private void label4_Click(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
