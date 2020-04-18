using System;
using System.Runtime.InteropServices;

namespace KeyboardMainBrowserWindowVideoPlaybackControl.ViewModel.Base
{
    public class BaseExternalLibraries : ViewModelBase
    {
        protected const UInt32 WM_KEYDOWN = 0x0100;
        protected const int VK_SPACE = 0x20;
        protected const int VK_ESC = 0x1B;
        protected const int VK_LEFT = 0x25;
        protected const int VK_RIGHT = 0x27;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        protected static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        protected static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        protected static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        protected static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        protected static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        protected static extern bool SendMessage(UInt32 HWND_BROADCAST, UInt32 WM_SYSCOMMAND, UInt32 SC_MONITORPOWER, int a);
    }
}
