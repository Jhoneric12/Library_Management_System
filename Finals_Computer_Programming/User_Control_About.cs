using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Finals_Computer_Programming
{
    public partial class User_Control_About : UserControl
    {
        public User_Control_About()
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
           //
        }

        private void User_Control_About_Load(object sender, EventArgs e)
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
        }

        private void timerTime_Tick_1(object sender, EventArgs e)
        {
            // Display date
            tssTime.Text = DateTime.Now.ToLongTimeString();
        }
    }
}
