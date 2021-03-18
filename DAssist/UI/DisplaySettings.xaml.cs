using DAssist.Domain;
using DAssist.Manager;
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
        private RampViewModel rampViewModel;

        public DisplaySettings()
        {
            InitializeComponent();

            rampViewModel = new RampViewModel();
            rampViewModel.PropertyChanged += OnViewModelPropertyChagned;
            DataContext = rampViewModel;

            DisplayRefreshButton.Click += (s, e) => RefreshDisplays();
        }

        /// <summary>
        /// 디스플레이 정보 새로고침
        /// </summary>
        public void RefreshDisplays()
        {
            rampViewModel.RefreshScreens();
        }

        /// <summary>
        /// RampViewModel 내부 데이터 변경 시 호출
        /// 선택된 디스플레이 밝기, 감마 조절
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnViewModelPropertyChagned(object sender, PropertyChangedEventArgs e)
        {
            Display display = DisplayItemsListBox.SelectedItem as Display;
            if (display != null)
            { 
                RampDisplay.UpdateRamp(display.Gamma, display.Brightness / 50.0f, display.DeviceName);
            }
        }
    }
}
