using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using global_keybord_hook;
using global_mouse_hook;

namespace Global_Keybord_Mouse_Hooking
{
    public partial class Form1 : Form
    {
        globalKeyboardHook gkh = new globalKeyboardHook();
        globalMouseHook gmh = new globalMouseHook();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gkh.is_hooking == false)
            {
                gkh.hook();
                gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
                gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
                label1.Text = "키보드 후킹중";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (gkh.is_hooking == true)
            {
                gkh.KeyDown -= new KeyEventHandler(gkh_KeyDown);
                gkh.KeyUp -= new KeyEventHandler(gkh_KeyUp);
                gkh.unhook();
                label1.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (gmh.is_mouse_hooking == false)
            {
                gmh.ButtonDown += new MouseEventHandler(gmh_ButtonDown);
                gmh.BottonUP += new MouseEventHandler(gmh_BottonUP);
                gmh._mouse_event += new MouseEventHandler(gmh__mouse_event);
               
                gmh.hook();
                label2.Text = "마우스 후킹중";
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (gmh.is_mouse_hooking == true)
            {
                gmh.ButtonDown -= new MouseEventHandler(gmh_ButtonDown);
                gmh.BottonUP -= new MouseEventHandler(gmh_BottonUP);
                gmh.unhook();
                label2.Text = "";
            }
        }

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            ListViewItem item = new ListViewItem(e.KeyCode.ToString());
            item.SubItems.Add("KeyUp");
            listView1.Items.Add(item);
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            ListViewItem item = new ListViewItem(e.KeyCode.ToString());
            item.SubItems.Add("KeyDown");
            listView1.Items.Add(item);
        }

        void gmh__mouse_event(object sender, MouseEventArgs e)
        {
            label3.Text = "X : " + e.X.ToString();
            label4.Text = "Y : " + e.Y.ToString();
        }

        void gmh_BottonUP(object sender, MouseEventArgs e)
        {
            ListViewItem item = new ListViewItem(e.Button.ToString());
            item.SubItems.Add("ButtonUP");
            listView2.Items.Add(item);
        }

      
        void gmh_ButtonDown(object sender, MouseEventArgs e)
        {
            ListViewItem item = new ListViewItem(e.Button.ToString());
            item.SubItems.Add("ButtonDown");
            listView2.Items.Add(item);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }
    }
}

