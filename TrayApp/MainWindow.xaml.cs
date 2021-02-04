using System.Windows;
using System.Windows.Forms;

namespace TrayApp
{
    public partial class MainWindow : Window
    {
        public NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (o, r) =>
            {
                // 아이콘 초기화
                notifyIcon = new NotifyIcon();
                notifyIcon.Icon = new System.Drawing.Icon(@"Graphicloads.ico");
                notifyIcon.ContextMenu = new ContextMenu();
                notifyIcon.Visible = true;

                // 예) 설정
                MenuItem itemSetting = new MenuItem();
                itemSetting.Index = 0;
                itemSetting.Text = "설정";
                itemSetting.Click += (s, e) => this.Show();
                notifyIcon.ContextMenu.MenuItems.Add(itemSetting);

                // 예) 종료
                MenuItem itemClose = new MenuItem();
                itemClose.Index = 1;
                itemClose.Text = "종료";
                itemClose.Click += (s, e) => System.Windows.Application.Current.Shutdown();
                notifyIcon.ContextMenu.MenuItems.Add(itemClose);

                // 아이콘 더블클릭시 윈도우 활성화
                notifyIcon.DoubleClick += (s, e) => this.Show();

                // 응용프로그램 종료시 아이콘 제거
                System.Windows.Application.Current.Exit += (s, e) => notifyIcon.Dispose();

                // 트레이버튼 클릭시 윈도우 비활성화
                ButtonTray.Click += (s, e) => this.Hide();
            };
        }
    }
}
