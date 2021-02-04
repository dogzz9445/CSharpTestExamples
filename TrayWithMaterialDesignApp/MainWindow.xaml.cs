using System;
using System.Windows;
using System.Windows.Controls;

namespace TrayWithMaterialDesignApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (o, r) =>
            {
                // 아이콘 초기화
                TrayTaskbarIcon.Icon = new System.Drawing.Icon(@"Graphicloads.ico");

                // 아이콘 더블클릭시 / 윈도우 활성화
                TrayTaskbarIcon.TrayMouseDoubleClick += (s, e) => this.Show();

                // 응용프로그램 종료시 / 아이콘 제거
                System.Windows.Application.Current.Exit += (s, e) => TrayTaskbarIcon.Dispose();

                // 설정 메뉴 클릭시 / 윈도우 활성화
                SettingsMenuItem.Click += (s, e) => this.Show();

                // 종료 메뉴 클릭시 / 응용프로그램 종료
                CloseMenuItem.Click += (s, e) => System.Windows.Application.Current.Shutdown();

                // 트레이 버튼 클릭시 / 윈도우 비활성화
                ButtonTray.Click += (s, e) => this.Hide();
            };
        }
    }
}
