using DrawTools;
using DrawTools.DB;
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

namespace AFC.MonitorMap.WPFTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //if (new StationConfigManager() != null)
            //{
            //    DBDeviceService db = new DBDeviceService(new StationConfigManager());
            //}
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitStationMapFormHost();
        }

        /// <summary>
        /// 初始化车站地图绘画窗体
        /// </summary>
        private void InitStationMapFormHost()
        {
            if (this.stationMapFormHost.Child == null)
            {
                StationMapForm mainForm = new StationMapForm();
                mainForm.Show();
                mainForm.TopLevel = false;
                mainForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.stationMapFormHost.Child = mainForm;
                mainForm.BringToFront();
            }
        }
    }
}
