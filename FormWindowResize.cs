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
        private Settings settings;

        public FormWindowResize(Settings settings)
        {
            InitializeComponent();

            this.settings = settings;

            switch (settings.WindowResizeType)
            {
                case Settings.WindowSizeTypes.w640h480 :
                    radioSize640.Checked = true;
                    break;
                case Settings.WindowSizeTypes.w800h600 :
                    radioSize800.Checked = true;
                    break;
                case Settings.WindowSizeTypes.w1024h768 :
                    radioSize1024.Checked = true;
                    break;
                default :
                    radioSizeCustom.Checked = true;
                    break;
            }
            upDownWidth.Value = settings.WindowResizeWidth;
            upDownHeight.Value = settings.WindowResizeHeight;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioSize640.Checked)
            {
                ResizeWindow(640, 480);
                settings.WindowResizeType = Settings.WindowSizeTypes.w640h480;
            }
            else if (radioSize800.Checked)
            {
                ResizeWindow(800, 600);
                settings.WindowResizeType = Settings.WindowSizeTypes.w800h600;
            }
            else if (radioSize1024.Checked)
            {
                ResizeWindow(1024, 768);
                settings.WindowResizeType = Settings.WindowSizeTypes.w1024h768;
            }
            else
            {
                ResizeWindow((int)upDownWidth.Value, (int)upDownHeight.Value);
                settings.WindowResizeType = Settings.WindowSizeTypes.custom;
            }

            settings.WindowResizeWidth = upDownWidth.Value;
            settings.WindowResizeHeight = upDownHeight.Value;

            this.Close();
        }

        private void ResizeWindow(int width, int height)
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
                    labelHeight.Enabled = upDownHeight.Enabled = sender.Equals(radioSizeCustom);
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
