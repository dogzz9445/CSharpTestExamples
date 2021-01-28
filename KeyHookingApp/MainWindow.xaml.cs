using System;
using System.Windows;
using System.Windows.Forms;

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
            if (0 < lastHotKeyId)
            {
                HotKeyManager.UnregisterHotKey(lastHotKeyId);
            }
        }

        public void HotKey_Pushed(object sender, HotKeyEventArgs e)
        {
            Console.WriteLine(e.KeyModifier.ToString() + "+" + e.Key.ToString());
        }
    }
}
