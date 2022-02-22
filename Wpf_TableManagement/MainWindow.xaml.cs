using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_TableManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var lang = MySettings.Default.language;
            CultureInfo.CurrentUICulture = new CultureInfo(lang);
            CultureInfo.CurrentCulture = new CultureInfo(lang);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            CBx_language.ItemsSource = MyResources.Resource.lnguages.Split(',');

        }

        private void Lbx_tables_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tbl = (Tables)Lbx_tables.SelectedItem;
            App.tableNo = tbl.tableNum;
            Window orderWin = new Window1();
            orderWin.Owner = this;
            orderWin.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void Tbx_filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = Tbx_filter.Text.ToLower();
            if (filter =="")
                Lbx_tables.ItemsSource = App._table;
            else
            {
                var lst = (from b in App._table where b.bookedName.ToLower().Contains(filter) select b).ToList();
                Lbx_tables.ItemsSource = lst;
            }
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            App._table = MyStorage.ReadXml<ObservableCollection<Tables>>("Tables.xml");
            foreach (Tables table in App._table)
            {
                if (table.bookedName != "")
                    table.status = "Wheat";
                else if (table.totalBill != 0.00)
                    table.status = "SandyBrown";
                else
                    table.status = "White";
            }
            Tbx_filter.Text = "";
            Lbx_tables.ItemsSource = App._table;
        }

        private void CBx_language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CBx_language.SelectedIndex)
            {
                case 0:
                    MySettings.Default.language = "en";
                    break;
                case 1:
                    MySettings.Default.language = "bn";
                    break;
                case 2:
                    MySettings.Default.language = "ar";
                    break;
                default:
                    MySettings.Default.language = "en";
                    break;
            }
            var msg = MyResources.Resource.restartAlert;
            var alert = MyResources.Resource.messageinfo;
            MessageBox.Show(msg,alert);
        }
    }
}