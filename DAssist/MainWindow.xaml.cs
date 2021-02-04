using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DAssist
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                /////////////////
                //
                // UI
                //
                /////////////////
                
                // 윈도우 창 위치
                var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
                Left = desktopWorkingArea.Right - this.Width;
                Top = desktopWorkingArea.Bottom - this.Height;
                // 아이콘 초기화
                //this.Icon = new System.Drawing.Icon(@"Graphicloads.ico");
                TrayTaskbarIcon.Icon = new System.Drawing.Icon(@"Graphicloads.ico");


                /////////////////
                //
                // 이벤트
                //
                /////////////////

                // 윈도우 어디를 선택해도 이동 가능
                MouseLeftButtonDown += (s, e) => this.DragMove();
                // 응용프로그램 종료시 아이콘 제거
                System.Windows.Application.Current.Exit += (s, e) => TrayTaskbarIcon.Dispose();
                // 메인 팝업 메뉴 트레이 클릭시 / 윈도우 비활성화
                MainMenuItemTray.Click += (s, e) => this.Hide();
                // 메인 팝업 메뉴 종료 클릭시 / 응용프로그램 종료
                MainMenuItemClose.Click += (s, e) => System.Windows.Application.Current.Shutdown();
                // 메인 메뉴 트레이 클릭시 / 윈도우 비활성화
                ButtonTray.Click += (s, e) => this.Hide();
                // 아이콘 더블클릭시 / 윈도우 활성화
                TrayTaskbarIcon.TrayMouseDoubleClick += (s, e) => this.Show();
                // 태스크바 아이콘 설정버튼 클릭시 / 윈도우 활성화
                (TrayTaskbarIcon.ContextMenu.Items.GetItemAt(1) as MenuItem).Click += (s, e) => this.Show();
                // 태스크바 아이콘 종료버튼 클릭시 / 응용프로그램 종료
                (TrayTaskbarIcon.ContextMenu.Items.GetItemAt(3) as MenuItem).Click += (s, e) => System.Windows.Application.Current.Shutdown();
            };
        }
            
        private void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is string stringValue)
            {
                try
                {
                    Clipboard.SetDataObject(stringValue);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
        }
    }
}
