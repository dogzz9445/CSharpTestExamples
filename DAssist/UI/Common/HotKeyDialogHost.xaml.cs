using DAssist.Domain;
using DAssist.Manager;
using DAssist.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using DAssist.Model;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;

namespace DAssist.UI.Common
{
    /// <summary>
    /// HotKeyDialogHost.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HotKeyDialogHost : System.Windows.Controls.UserControl
    {
        private HotKeyViewModel HotKeyViewModel;

        private int HotKeyRegisteredId;
        private HotKey PreviousHotKey;

        public event EventHandler<HotKeyEventArgs> HotKeyPressed;

        public HotKeyDialogHost()
        {
            InitializeComponent();

            HotKeyRegisteredId = -1;

            // TODO:
            // Configuration 불러오기


            // HotKey 뷰모델
            HotKeyViewModel = new HotKeyViewModel();
            this.DataContext = HotKeyViewModel;

            Loaded += (s, e) => RegisterHotKey();
            HotKeyViewModel.PropertyChanged += (s, e) => RegisterHotKey();
        }

        public async void RegisterHotKey()
        {
            UnRegisterHotKey();

            Keys key = HotKeyViewModel.PropertyHotKey.ActionKey;
            KeyModifier keyModifier = HotKeyViewModel.PropertyHotKey.GetKeyModifier;
            var (ret, id) = await HotKeyManager.RegisterHotKey(key, keyModifier);
            HotKeyRegisteredId = id;
            HotKeyManager.HotKeyPressed += OnHotKeyPressed;
        }

        public void UnRegisterHotKey()
        {
            if (HotKeyRegisteredId > -1)
            {
                HotKeyManager.HotKeyPressed -= OnHotKeyPressed;
                HotKeyManager.UnregisterHotKey(HotKeyRegisteredId);
                HotKeyRegisteredId = -1;
            }
        }

        private void OnDialogHostOpened(object sender, DialogOpenedEventArgs eventArgs)
        {
            PreviousHotKey = HotKeyViewModel.PropertyHotKey;

            HotKeyViewModel.PropertyChanged -= (s, e) => RegisterHotKey();
            UnRegisterHotKey();
        }

        private void OnDialogHostClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true))
            {
                if (PreviousHotKey != null)
                {
                    HotKeyViewModel.PropertyHotKey = PreviousHotKey;
                }
            }

            HotKeyViewModel.PropertyChanged += (s, e) => RegisterHotKey();
        }

        private void OnHotKeyPressed(object sender, HotKeyEventArgs eventArgs)
        {
            HotKey hotKey = HotKeyViewModel.PropertyHotKey;
            if ((eventArgs.Key == hotKey.ActionKey) && (eventArgs.KeyModifier == hotKey.GetKeyModifier))
            {
                HotKeyPressed?.Invoke(this, eventArgs);
            }
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs eventArgs)
        {
            (sender as System.Windows.Controls.TextBox).Text = eventArgs.Key.ToString();
        }
    }
}
