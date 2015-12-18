using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PSOBBTools
{
    public partial class FormWindowResize : Form
    {
        public FormWindowResize()
        {
            InitializeComponent();

            getClientSize();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioSize640.Checked)
            {
                resizeWindow(640, 480);
            }
            else if (radioSize800.Checked)
            {
                resizeWindow(800, 600);
            }
            else if (radioSize1024.Checked)
            {
                resizeWindow(1024, 768);
            }
            else
            {
                resizeWindow((int)upDownWidth.Value, (int)upDownHeight.Value);
            }

            this.Close();
        }

        private void getClientSize()
        {
            Window.RECT rect;

            IntPtr hwnd = Window.FindWindow(Settings.windowClassName, null);

            if (hwnd != IntPtr.Zero)
            {
                if (Window.GetClientRect(hwnd, out rect))
                {
                    upDownWidth.Value = rect.right;
                    upDownHeight.Value = rect.bottom;
                }
            }
        }

        private void resizeWindow(int width, int height)
        {
            IntPtr hwnd = Window.FindWindow(Settings.windowClassName, null);

            if (hwnd != IntPtr.Zero)
            {
                Window.WindowStyleFlags style = (Window.WindowStyleFlags)Window.GetWindowLong(hwnd, Window.GetWindowLongFlags.GWL_STYLE);
                int exStyle = Window.GetWindowLong(hwnd, Window.GetWindowLongFlags.GWL_EXSTYLE);
                Window.RECT rect;

                rect.left = rect.top = 0;
                rect.right = width;
                rect.bottom = height;

                if (Window.AdjustWindowRectEx(ref rect, style, false, (uint)exStyle))
                {
                    Window.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, rect.right - rect.left, rect.bottom - rect.top,
                        Window.SetWindowPosFlags.SWP_NOACTIVATE |
                        Window.SetWindowPosFlags.SWP_NOMOVE |
                        Window.SetWindowPosFlags.SWP_NOOWNERZORDER |
                        Window.SetWindowPosFlags.SWP_NOZORDER);
                }
            }
        }

        private void radioSize_CheckedChanged(object sender, EventArgs e)
        {
                labelWidth.Enabled = upDownWidth.Enabled =
                    labelHeight.Enabled = upDownHeight.Enabled = sender.Equals(radioSize);
        }

        private void upDown_Enter(object sender, EventArgs e)
        {
            if (sender is NumericUpDown)
            {
                NumericUpDown numUpDown = (NumericUpDown)sender;
                numUpDown.Select(0, numUpDown.Value.ToString().Length);
            }
        }
    }
}
