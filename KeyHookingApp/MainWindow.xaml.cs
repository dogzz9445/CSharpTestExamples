using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace KeyHookingApp
{
    public partial class MainWindow : Window
    {
        private int lastHotKeyId = 0;

        public MainWindow()
        {
            InitializeComponent();
            HotKeyManager.HotKeyPressed += HotKey_Pushed;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var (ret, id) = await HotKeyManager.RegisterHotKey(Keys.A, KeyModifier.Alt);
            (ret, id) = await HotKeyManager.RegisterHotKey(Keys.S, KeyModifier.Alt);

            lastHotKeyId = id;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            while (0 < lastHotKeyId)
            {
                HotKeyManager.UnregisterHotKey(lastHotKeyId--);
            }
        }

        public void HotKey_Pushed(object sender, HotKeyEventArgs e)
        {
            textBlockPressedKey.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                textBlockPressedKey.Text = e.KeyModifier.ToString() + "+" + e.Key.ToString();
            }));
        }
    }
}
