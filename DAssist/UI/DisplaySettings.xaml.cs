using DAssist.Domain;
using DAssist.Manager;
using DAssist.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DAssist.UI
{
    /// <summary>
    /// DisplaySettings.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DisplaySettings : UserControl
    {
        private RampViewModel DisplayRampViewModel;

        //public event EventHandler<HotKeyEventArgs> HotKeyPressed;

        public DisplaySettings()
        {
            InitializeComponent();

            // Ramp 뷰모델
            DisplayRampViewModel = new RampViewModel();
            DisplayItemsListBox.DataContext = DisplayRampViewModel;

            // Button Event
            DisplayRefreshButton.Click += (s, e) => DisplayRampViewModel.RefreshScreens();

        }
    }
}
