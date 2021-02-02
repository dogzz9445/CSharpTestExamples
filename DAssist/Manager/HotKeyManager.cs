using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAssist.Manager
{
    [Flags]
    public enum KeyModifier
    {
        None = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        Shift = 0x0004,
        Win = 0x0008,
        NoRepeat = 0x4000
    }

    public class HotKeyEventArgs : EventArgs
    {
        public HotKeyEventArgs(KeyModifier keyModifier, Keys key)
        {
            KeyModifier = keyModifier;
            Key = key;
        }

        public HotKeyEventArgs(IntPtr hotKeyParam)
        {
            uint param = (uint)hotKeyParam.ToInt64();
            Key = (Keys)((param & 0xffff0000) >> 16);
            KeyModifier = (KeyModifier)(param & 0x0000ffff);
        }

        public KeyModifier KeyModifier { get; private set; }
        public Keys Key { get; private set; }
    }

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
