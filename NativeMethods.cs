using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MuAutoReset
{

    class NativeMethods
    {

        public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int BM_SETSTATE = 0x00F3;

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public static List<IntPtr> GetAllChildrenWindowHandles(IntPtr hParent, int maxCount)
        {
            List<IntPtr> result = new List<IntPtr>();
            int ct = 0;
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            while (true && ct < maxCount)
            {
                currChild = FindWindowEx(hParent, prevChild, null, null);
                if (currChild == IntPtr.Zero)
                    break;
                result.Add(currChild);
                prevChild = currChild;
                ++ct;
            }
            return result;
        }

        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern IntPtr ChildWindowFromPointEx(IntPtr hWnd, Point pt, uint uFlags);

        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(HookType code, HookProc func, IntPtr hInstance, int threadID);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr hhook);

        public static int MakeLParam(int LoWord, int HiWord)
        {
            return (int)((HiWord << 16) | (LoWord & 0xFFFF));
        }

        public static void MoveCursor(IntPtr hWnd, int x, int y)
        {
            PostMessage(hWnd, WM_MOUSEMOVE, 0, MakeLParam(x, y));
        }

        public static void MoveCursor(Process process, int x, int y)
        {
            MoveCursor(process.MainWindowHandle, x, y);
        }

        public static void MakeClick(IntPtr hWnd, int x, int y)
        {
            MoveCursor(hWnd, x, y);
            PostMessage(hWnd, WM_LBUTTONDOWN, 1, MakeLParam(x, y));
            Thread.Sleep(100);
            PostMessage(hWnd, WM_LBUTTONUP, 0, MakeLParam(x, y));
        }

        public static void MakeClick(Process process, int x, int y)
        {
            MoveCursor(process, x, y);
            MakeClick(process.MainWindowHandle, x, y);
        }

    }

}

//LRESULT CALLBACK WinProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
//{
//    if (clicker)
//    {
//        switch (uMsg)
//        {
//            case WM_ACTIVATE:
//                if (LOWORD(wParam) == WA_INACTIVE)
//                    return NULL;
//                break;
//            case WM_NCACTIVATE:
//                if (!(BOOL)wParam)
//                    return NULL;
//                break;
//        }
//    }

//    return CallWindowProc(WndProcOriginal, hWnd, uMsg, wParam, lParam);
//}