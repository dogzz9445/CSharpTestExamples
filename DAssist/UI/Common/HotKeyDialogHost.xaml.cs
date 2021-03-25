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
            // FIXME:
            // 제대로 동작안함
            PreviousHotKey = HotKeyViewModel.PropertyHotKey;
        }

        private void OnDialogHostClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            // FIXME:
            // 제대로 동작안함
            if (!Equals(eventArgs.Parameter, true))
            {
                HotKeyViewModel.PropertyHotKey.ActionKey = PreviousHotKey.ActionKey;
                HotKeyViewModel.PropertyHotKey.FirstKeyModifier = PreviousHotKey.FirstKeyModifier;
                HotKeyViewModel.PropertyHotKey.SecondKeyModifier = PreviousHotKey.SecondKeyModifier;
            }
        }

        private void OnHotKeyPressed(object sender, HotKeyEventArgs eventArgs)
        {
            HotKey hotKey = HotKeyViewModel.PropertyHotKey;
            if ((eventArgs.Key == hotKey.ActionKey) && (eventArgs.KeyModifier == hotKey.GetKeyModifier))
            {
                HotKeyPressed?.Invoke(this, eventArgs);
            }
        }

        private void OnKeyModifierDown(object sender, System.Windows.Input.KeyEventArgs eventArgs)
        {
            // FIXME:
            // KeyModifier만 걸러내는 작업

            //Keys key = (Keys)KeyInterop.VirtualKeyFromKey(eventArgs.Key);
            //(sender as System.Windows.Controls.TextBox).Text = key.ToString();
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs eventArgs)
        {
            // FIXME:
            // 가능한 0~9까지나 다른 글자도 받을수 있게
            if (eventArgs.Key >= Key.A && eventArgs.Key <= Key.Z)
            {
                Keys key = (Keys)KeyInterop.VirtualKeyFromKey(eventArgs.Key);
                (sender as System.Windows.Controls.TextBox).Text = key.ToString();
            }
        }
    }
}
