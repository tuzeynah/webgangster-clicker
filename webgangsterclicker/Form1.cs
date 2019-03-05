using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace webgangsterclicker
{
    public partial class Form1 : Form
    {
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetAsyncKeyState(int key);
        [DllImport("user32", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll", EntryPoint = "mouse_event", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern bool apimouse_event(int dwFlags, int dX, int dY, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private int toggle;
        private Random rnd;
        public Point mouseLocation;
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0016;

        public Form1()
        {
            this.rnd = new Random();
            InitializeComponent();
            int FirstHotkeyId = 1;
            int FirstHotKeyKey = (int)Keys.F4;
            bool F4Registered = RegisterHotKey(Handle, FirstHotkeyId, 0x0000, FirstHotKeyKey);

            if (!F4Registered)
            {
                Console.WriteLine("Global Hotkey F4 couldn't be registered !");
            }
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();
                switch (id)
                {
                    case 1:
                        autoclicker(this, new EventArgs());
                        break;
                }
            }

            base.WndProc(ref m);
        }

        private void autoclicker(object sender, EventArgs e)
        {
            checked { ++this.toggle; }
            if (this.toggle == 1)
            {
                this.label2.Text = "ON";
                timer1.Start();
            }
            else
            {
                this.label2.Text = "OFF";
                timer1.Stop();
                this.toggle = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(MouseButtons == MouseButtons.Left)
            {
                int maxValue = checked((int)Math.Round(unchecked(1000.0 / (double)this.trackBar1.Value + (double)this.trackBar1.Value * 0.2)));
                int minValue = checked((int)Math.Round(unchecked(1000.0 / (double)this.trackBar2.Value + (double)this.trackBar2.Value * 0.48)));

                this.timer1.Interval = this.rnd.Next(maxValue, minValue);

                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }
    }
}
