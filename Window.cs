using System;
using System.Runtime.InteropServices;   // DllImport

namespace PSOBBTools
{
    class Window
    {
        [Flags]
        public enum GetWindowLongFlags : int
        {
            GWL_WNDPROC     = (-4),
            GWL_HINSTANCE   = (-6),
            GWL_HWNDPARENT  = (-8),
            GWL_STYLE       = (-16),
            GWL_EXSTYLE     = (-20),
            GWL_USERDATA    = (-21),
            GWL_ID          = (-12)
        }

        [Flags]
        public enum WindowStyleFlags : uint
        {
            WS_OVERLAPPED       = 0x00000000,
            WS_POPUP            = 0x80000000,
            WS_CHILD            = 0x40000000,
            WS_MINIMIZE         = 0x20000000,
            WS_VISIBLE          = 0x10000000,
            WS_DISABLED         = 0x08000000,
            WS_CLIPSIBLINGS     = 0x04000000,
            WS_CLIPCHILDREN     = 0x02000000,
            WS_MAXIMIZE         = 0x01000000,
            WS_CAPTION          = 0x00C00000,
            WS_BORDER           = 0x00800000,
            WS_DLGFRAME         = 0x00400000,
            WS_VSCROLL          = 0x00200000,
            WS_HSCROLL          = 0x00100000,
            WS_SYSMENU          = 0x00080000,
            WS_THICKFRAME       = 0x00040000,
            WS_GROUP            = 0x00020000,
            WS_TABSTOP          = 0x00010000,

            WS_MINIMIZEBOX      = 0x00020000,
            WS_MAXIMIZEBOX      = 0x00010000,

            WS_TILED            = WS_OVERLAPPED,
            WS_ICONIC           = WS_MINIMIZE,
            WS_SIZEBOX          = WS_THICKFRAME,
            WS_TILEDWINDOW      = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),
            WS_POPUPWINDOW      = (WS_POPUP | WS_BORDER | WS_SYSMENU),
            WS_CHILDWINDOW      = WS_CHILD
        }

        [Flags]
        public enum SetWindowPosFlags : uint
        {
            SWP_NOSIZE          = 0x0001,
            SWP_NOMOVE          = 0x0002,
            SWP_NOZORDER        = 0x0004,
            SWP_NOREDRAW        = 0x0008,
            SWP_NOACTIVATE      = 0x0010,
            SWP_FRAMECHANGED    = 0x0020,
            SWP_SHOWWINDOW      = 0x0040,
            SWP_HIDEWINDOW      = 0x0080,
            SWP_NOCOPYBITS      = 0x0100,
            SWP_NOOWNERZORDER   = 0x0200,
            SWP_NOSENDCHANGING  = 0x0400,

            SWP_DRAWFRAME       = SWP_FRAMECHANGED,
            SWP_NOREPOSITION    = SWP_NOOWNERZORDER,

            SWP_DEFERERASE      = 0x2000,
            SWP_ASYNCWINDOWPOS  = 0x4000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, GetWindowLongFlags nIndex);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, GetWindowLongFlags nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, WindowStyleFlags dwStyle, bool bMenu, uint dwExStyle);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
    }
}
