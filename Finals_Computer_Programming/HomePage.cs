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
    public partial class HomePage : Form
    {
        int PanelWidth;
        bool hidden;

        public HomePage(string name, string lName, string userType)
        {
            InitializeComponent();
            // Show user's name 
            lblUser.Text = name + " " + lName;
            lblUserType.Text = userType;
            User_Control_Home home = new User_Control_Home();
            AddControlsToMainPanel(home);
            PanelWidth = panelLeft.Width;
            hidden = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogres = MessageBox.Show("Are you sure you want to Log out?", "University of Rizal System Binangonan Library Management", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogres == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Slide panelLeft
            if (hidden)
            {
                panelLeft.Width = panelLeft.Width + 10;
                if (panelLeft.Width >= PanelWidth)
                {
                    timer1.Stop();
                    hidden = false;
                    this.Refresh();
                }
            }
            else
            {
                panelLeft.Width = panelLeft.Width - 10;
                if (panelLeft.Width <= 59)
                {
                    timer1.Stop();
                    hidden = true;
                    this.Refresh();
                }
            }
        }

        private void btnMinimized_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        // This method is to move PanelSide
        private void MovePanelSide(Control btn)
        {
            panelSide.Top = btn.Top;
            panelSide.Height = btn.Height;
        }

        // This method is to add controls from other form to Main Panel
        private void AddControlsToMainPanel(Control ctrl)
        {
            ctrl.Dock = DockStyle.Fill;
            MainPanelConrols.Controls.Clear();
            MainPanelConrols.Controls.Add(ctrl);
        }

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            MovePanelSide(btnDashBoard);
            User_Control_Home home = new User_Control_Home();
            AddControlsToMainPanel(home);
        }

        private void btnViewStudents_Click(object sender, EventArgs e)
        {
            MovePanelSide(btnViewStudents);
            User_Control_View_Students student = new User_Control_View_Students();
            AddControlsToMainPanel(student);
        }

        private void btnLibrary_Click(object sender, EventArgs e)
        {
            MovePanelSide(btnLibrary);
            User_Control_Library library = new User_Control_Library();
            AddControlsToMainPanel(library);
        }

        private void btnBorrowBooks_Click(object sender, EventArgs e)
        {
            MovePanelSide(btnBorrowBooks);
            User_Control_Borrow_Book borrow = new User_Control_Borrow_Book();
            AddControlsToMainPanel(borrow);
        }

        private void btnReturnBooks_Click(object sender, EventArgs e)
        {
            MovePanelSide(btnReturnBooks);
            User_Control_Return_Books returnBooks = new User_Control_Return_Books();
            AddControlsToMainPanel(returnBooks);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MovePanelSide(btnAbout);
            User_Control_About about = new User_Control_About();
            AddControlsToMainPanel(about);
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
           //
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            //
        }

        private void btnDeletedRecords_Click(object sender, EventArgs e)
        {
            MovePanelSide(btnDeletedRecords);
            User_Control_Deleted_Records deleted = new User_Control_Deleted_Records();
            AddControlsToMainPanel(deleted);
        }
    }
}
