using DAssist.Model;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAssist.Manager
{
    public static class HotKeyManager
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private class MessageWindow : Form
        {
            private const int WM_HOTKEY = 0x312;

            public MessageWindow()
            {
                _wnd = this;
                _hwnd = this.Handle;
                _windowReadyEvent.Set();
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_HOTKEY)
                {
                    HotKeyEventArgs e = new HotKeyEventArgs(m.LParam);
                    HotKeyManager.OnHotKeyPressed(this, e);
                }

                base.WndProc(ref m);
            }

            protected override void SetVisibleCore(bool value)
            {
                base.SetVisibleCore(false);
            }
        }

        private static int _id;
        private static volatile MessageWindow _wnd;
        private static volatile IntPtr _hwnd;
        private static ManualResetEvent _windowReadyEvent = new ManualResetEvent(false);

        static HotKeyManager()
        {
            new Thread(() =>
            {
                Application.Run(new MessageWindow());
            })
            {
                Name = "MessageLoopThread",
                IsBackground = true
            }.Start();
        }

        public static async Task<(bool, int)> RegisterHotKey(Keys key, KeyModifier keyModifier)
        {
            await Task.Run(() => { _windowReadyEvent.WaitOne(); });
            int id = Interlocked.Increment(ref _id);
            bool ret = (bool)_wnd.Invoke(new Func<IntPtr, int, uint, uint, bool>(RegisterHotKey), _hwnd, id, (uint)keyModifier, (uint)key);
            return (ret, id);
        }

        public static bool UnregisterHotKey(int id)
        {
            return (bool)_wnd.Invoke(new Func<IntPtr, int, bool>(UnregisterHotKey), _hwnd, id);
        }

        public static event EventHandler<HotKeyEventArgs> HotKeyPressed;

        private static void OnHotKeyPressed(object sender, HotKeyEventArgs eventData)
        {
            HotKeyPressed?.Invoke(sender, eventData);
        }
    }
}
