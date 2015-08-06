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
using Microsoft.Windows.Appx.PackageManager.Commands;

namespace WindowsAppUninstaller
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private AppxPackage[] apps;
        private BindingList<KeyValuePair<string, string>> dic;

        public MainWindow()
        {
            InitializeComponent();
            InitializeEventHandlers();
        }

        void InitializeEventHandlers()
        {
            this.Loaded += MainWindow_Loaded;
            btnGetApps.Click += BtnGetApps_Click;
            btnRemoveApp.Click += BtnRemoveApp_Click;
            lstApps.SelectionChanged += LstApps_SelectionChanged;
        }

        private void LstApps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppxPackage item;
            if (e.AddedItems.Count <= 0 && e.RemovedItems.Count <= 0)
            {
                return;
            }
            if (e.AddedItems.Count > 0)
            {
                item = e.AddedItems[0] as AppxPackage;
            }
            else
            {
                item = e.RemovedItems[0] as AppxPackage;
            }
            //var item = lvProps.SelectedItem as AppxPackage;
            if (item != null)
            {
                string k, v;
                var props = typeof(AppxPackage).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Instance);
                if (dic == null)
                {
                    dic = new BindingList<KeyValuePair<string, string>>();
                }
                dic.Clear();
                for (var i = 0; i < props.Length; i++)
                {
                    k = props[i].Name;
                    v = props[i].GetValue(item) as string;
                    if (v != null)
                    {
                        dic.Add(new KeyValuePair<string, string>(k, v));
                    }
                }

                lvProps.ItemsSource = dic;
            }

            btnRemoveApp.IsEnabled = lstApps.SelectedItems.Count > 0;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            btnRemoveApp.IsEnabled = false;
            statusText.Text = "就绪";
        }

        private void BtnRemoveApp_Click(object sender, RoutedEventArgs e)
        {
            var r = MessageBox.Show("您确认要删除 " + lstApps.SelectedItems.Count.ToString() + " 个 App 吗？", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (r == MessageBoxResult.No)
            {
                return;
            }

            var l = new List<AppxPackage>();
            foreach (var k in lstApps.SelectedItems)
            {
                l.Add(k as AppxPackage);
            }

            int failedCount = 0;
            for (var i = 0; i < l.Count; i++)
            {
                statusText.Text = string.Format("正在删除第 {0}/{1} 项: {2}", i + 1, l.Count, l[i].Name);
                try
                {
                    AppxManager.RemovePackage(l[i].PackageFullName, new AppxParameters());
                }
                catch (Exception)
                {
                    failedCount++;
                }
            }

            statusText.Text = "就绪";
            string prompt;
            if (failedCount == 0)
            {
                prompt = $"已删除 {l.Count} 项。";
            }
            else
            {
                prompt = $"已删除 {l.Count - failedCount} 项，失败 {failedCount} 项，总计 {l.Count} 项。";
            }
            MessageBox.Show(prompt, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnGetApps_Click(object sender, RoutedEventArgs e)
        {
            statusText.Text = "正在搜索...";
            try
            {
                var xapps = AppxManager.GetPackages(new AppxParameters());
                if (xapps?.Length > 0)
                {
                    apps = xapps;

                    lstApps.ItemsSource = apps;

                    btnRemoveApp.IsEnabled = lstApps.SelectedItems.Count > 0;
                }
            }
            catch (Exception ex)
            {
                statusText.Text = ex.Message;
            }
            finally
            {
                statusText.Text = "就绪";
            }
        }
    }
}
