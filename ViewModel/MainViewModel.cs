using Gma.System.MouseKeyHook;
using KeyboardMainBrowserWindowVideoPlaybackControl.Enums;
using KeyboardMainBrowserWindowVideoPlaybackControl.ViewModel.Base;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using WpfControls = System.Windows.Controls;

namespace KeyboardMainBrowserWindowVideoPlaybackControl.ViewModel
{
    public class MainViewModel : BaseExternalLibraries
    {
        private const string BUTTON_START = "Start";
        private const string BUTTON_STOP = "Stop";

        private IKeyboardMouseEvents _globalHook;

        public ICommand StartCommand { get; set; }
        public MainViewModel()
        {
            StartCommand = new RelayCommand<WpfControls.Button>((startButton) => Start(startButton));
        }

        private void Start(WpfControls.Button startButton)
        {
            if (startButton.Content.Equals(BUTTON_START)) 
            {
                SubscribeToEvents();
            }
            else
            {
                Unsubscribe();
            }

            startButton.Content = ToggleButtonText(startButton);
        }

        private object ToggleButtonText(WpfControls.Button startButton)
        {
            return startButton.Content.Equals(BUTTON_START)
                ? BUTTON_STOP
                : BUTTON_START;
        }

        private void SubscribeToEvents()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.MouseDownExt += GlobalHookMouseDownExt;
            _globalHook.KeyDown += _keyboardMouseEvents_KeyDown;
        }

        public void Unsubscribe()
        {
            _globalHook.MouseDownExt -= GlobalHookMouseDownExt;
            _globalHook.KeyDown += _keyboardMouseEvents_KeyDown;

            _globalHook.Dispose();
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                SendValueToProcessAndFocusPrevious(EKey.Default);
            }
        }
        private void _keyboardMouseEvents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SendValueToProcessAndFocusPrevious(EKey.Default);
            }
            else if (e.KeyCode == Keys.F1)
            {
                SendValueToProcessAndFocusPrevious(EKey.Backward);
            }
            else if (e.KeyCode == Keys.F2)
            {
                SendValueToProcessAndFocusPrevious(EKey.Forward);
            }
        }

        private Process GetCurrentFocusedWindow()
        {
            IntPtr hwnd = GetForegroundWindow();

            var id = GetWindowThreadProcessId(hwnd, out uint pid);
            return Process.GetProcessById((int)pid);
        }

        private void SendValueToForegroundWindow(EKey key)
        {
            var processes = Process.GetProcessesByName("chrome");
            var process = processes.Single(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));
            Thread.Sleep(100);
            SetForegroundWindow(process.MainWindowHandle);
            CreatePostMessage(key, process);
        }
        void CreatePostMessage(EKey key, Process process)
        {
            Thread.Sleep(100);
            switch (key)
            {
                case EKey.Default:
                    PostMessage(process.MainWindowHandle, WM_KEYDOWN, VK_SPACE, 0);
                    break;
                case EKey.Backward:
                    PostMessage(process.MainWindowHandle, WM_KEYDOWN, VK_LEFT, 0);
                    break;
                case EKey.Forward:
                    PostMessage(process.MainWindowHandle, WM_KEYDOWN, VK_RIGHT, 0);
                    break;
                case EKey.Escape:
                    PostMessage(process.MainWindowHandle, WM_KEYDOWN, VK_ESC, 0);
                    break;
                default:
                    throw new Exception();
            }
        }

        private void SendValueToProcessAndFocusPrevious(EKey key)
        {
            var process = GetCurrentFocusedWindow();
            SendValueToForegroundWindow(key);
            Thread.Sleep(100);
            SetForegroundWindow(process.MainWindowHandle);
            CreatePostMessage(EKey.Escape, process);
        }
    }
}
