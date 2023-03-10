using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace global_mouse_hook
{
    class globalMouseHook
    {
        #region DLL IMPORT

       
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        static extern bool CloseWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, mouseHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref MSLLHOOKSTRUCT lParam);

        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte vk, byte scan, int flags, ref  int extrainfo);

        #endregion

        public bool is_mouse_hooking = false;
        IntPtr hhook_mouse = IntPtr.Zero;
        mouseHookProc mhp;


        #region Flags
        public struct MSLLHOOKSTRUCT
        {
            public int x;//x좌표
            public int y;//y좌표
            public int flags;
            public int time;
            public int dwExtraInfo;

        }
                
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_MOUSEMOVE = 0x0200;
        const int WM_MOUSEWHEEL = 0x020A;
        const int WM_RBUTTONDOWN = 0x0204;
        const int WM_RBUTTONUP = 0x0205;
        const int WH_MOUSE_LL = 14; 
    
        #endregion


        public delegate int mouseHookProc(int code, int wParam, ref MSLLHOOKSTRUCT lParam);

        public event MouseEventHandler ButtonDown;

        public event MouseEventHandler BottonUP;
        public event MouseEventHandler _mouse_event;


        public int hookproc_mouse(int code, int wParam, ref MSLLHOOKSTRUCT lParam)
        {
            MouseButtons b = new MouseButtons();
            MouseEventArgs e;

            
            switch (wParam)
            {
                case WM_LBUTTONDOWN:

                    b = MouseButtons.Left;
                    e = new MouseEventArgs(b, 1, lParam.x, lParam.y, 0);
                    ButtonDown(this,e);
                    break;

                case WM_LBUTTONUP :
                    b = MouseButtons.Left;
                    e = new MouseEventArgs(b, 1, lParam.x, lParam.y, 0);
                    BottonUP(this,e);
                    break;

                case WM_RBUTTONDOWN :
                    b = MouseButtons.Right;
                    e = new MouseEventArgs(b, 1, lParam.x, lParam.y, 0);
                    ButtonDown(this,e);
                    break;

                case WM_RBUTTONUP :
                    b = MouseButtons.Right;
                    e = new MouseEventArgs(b, 1, lParam.x, lParam.y, 0);
                 
                    BottonUP(this,e);
                    break;
               case WM_MOUSEMOVE :
                    b = MouseButtons.None;
                    e = new MouseEventArgs(b, 1, lParam.x, lParam.y, 0);
                    _mouse_event(this, e);
                    break;
            }

            return CallNextHookEx(hhook_mouse, code, wParam, ref lParam); //나머지 윈도우에서 자동으로 처리 중요!!
        }

        #region Constructors and Destructors

        public globalMouseHook()
        {
            mhp = new mouseHookProc(hookproc_mouse);
           // hook();
        }

        ~globalMouseHook()
        {
            unhook();
        }
        #endregion

        public void hook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook_mouse = SetWindowsHookEx(WH_MOUSE_LL, mhp, hInstance, 0);
            is_mouse_hooking = true;
        }

        public void unhook()
        {
            UnhookWindowsHookEx(hhook_mouse);
            is_mouse_hooking = false;
        }
    }
}
