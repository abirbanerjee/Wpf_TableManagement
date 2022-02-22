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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Wpf_TableManagement
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    
    public partial class Window1 : Window
    {
        public Window1()
        {
            var lang = MySettings.Default.language;
            CultureInfo.CurrentUICulture = new CultureInfo(lang);
            CultureInfo.CurrentCulture = new CultureInfo(lang);
            InitializeComponent();
        }

        public bool writeNew = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App._order = MyStorage.ReadXml<ObservableCollection<Orders>>("Orders.xml");
            if (App._order == null)
            {
                App._order = new ObservableCollection<Orders>();                
            }
            var tableOrders = (from b in App._order where b.tableNo == App.tableNo select b).ToList();
            Lbx_order.ItemsSource = tableOrders.Distinct();
            Tbk_table.Text = App.tableNo.ToString();
            Cbx_MenuItems.ItemsSource = MyResources.Resource.menuItems.Split(',');
            Cbx_MenuItems.SelectedIndex = 0;
            foreach(Tables tab in App._table)
                if(tab.tableNum == App.tableNo)
                    Tbk_total_price.Text=tab.totalBill.ToString();
    }

        public void Cbx_MenuItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var filter = "";
            var sel = Cbx_MenuItems.SelectedIndex;
            switch(sel)
            {
                case 0:
                    filter = "Drinks";
                    break;
                case 1:
                    filter = "Mains";
                    break ;
                case 2:
                    filter = "Desserts";
                    break;
                default:
                    filter = "Drinks";
                    break;
                   
            }
            filter = filter.ToLower();
            if (filter == "")
                Lbx_menu.ItemsSource = App._menu;
            else
            {
                var lst = (from b in App._menu where b.category.ToLower() ==filter select b).ToList();
                Lbx_menu.ItemsSource=lst;
            }
        }

        private void ManageOrders_Closed(object sender, EventArgs e)
        {
            MyStorage.WriteXml(App._order, "Orders.xml");
            MyStorage.WriteXml(App._table, "Tables.xml");
            Owner.Visibility = Visibility.Visible;
        }

        private void Lbx_menu_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var menuSelected = (ResMenu)Lbx_menu.SelectedItem;
            var checkEmpty = (from b in App._order where b.tableNo == App.tableNo select b).ToList();
            if (checkEmpty.Count == 0)
            {
                var order = new Orders { tableNo = App.tableNo, orderedDishes = menuSelected.name, count=1, totalPrice=menuSelected.price };
                App._order.Add(order);
            }

            else
            {
                foreach (Orders ord in App._order)
                {
                    if (ord.tableNo == App.tableNo && ord.orderedDishes == menuSelected.name)
                    {
                        ord.count++;
                        ord.totalPrice = ord.count * menuSelected.price;
                        writeNew = false;
                        break;
                        
                    }
                    else 
                    {
                        writeNew = true;
                    }
                        
                }
            }
            if (writeNew)
            {
                var order = new Orders { tableNo = App.tableNo, orderedDishes = menuSelected.name, count=1, totalPrice=menuSelected.price };
                App._order.Add(order);
                writeNew = false;
            }
            foreach (Tables prc in App._table)
            {
                if(prc.tableNum == App.tableNo)
                {
                    prc.totalBill += menuSelected.price;
                    Tbk_total_price.Text = prc.totalBill.ToString(".00");
                }
            }

            var tableOrders = (from b in App._order where b.tableNo == App.tableNo select b).ToList();
            Lbx_order.ItemsSource = tableOrders.Distinct();
        }

        private void Btn_rmv_Click(object sender, RoutedEventArgs e)
        {
            var itm =(Orders) Lbx_order.SelectedItem;
            if(itm == null)
            {
                var msg = MyResources.Resource.selToDel;
                var war = MyResources.Resource.messageinfo;
                MessageBox.Show(msg,war);
                return;
            }
            if(itm.count==1)
            {
                foreach (Tables prc in App._table)
                {
                    if (prc.tableNum == App.tableNo)
                    {
                        prc.totalBill -= itm.totalPrice;
                        Tbk_total_price.Text = prc.totalBill.ToString(".00");
                    }
                }
                App._order.Remove(itm);

            }
            else
            {
                var unitPrice = itm.totalPrice/itm.count;
                itm.totalPrice-=unitPrice;
                itm.count--;
                foreach (Tables prc in App._table)
                {
                    if (prc.tableNum == App.tableNo)
                    {
                        prc.totalBill -= unitPrice;
                        Tbk_total_price.Text = prc.totalBill.ToString(".00");
                    }
                }
            }
            var tableOrders = (from b in App._order where b.tableNo == App.tableNo select b).ToList();
            Lbx_order.ItemsSource = tableOrders.Distinct();
        }

        private void Btn_pay_Click(object sender, RoutedEventArgs e)
        {
            var payable = Tbk_total_price.Text;
            var totbill = MyResources.Resource.totBill + payable;
            var confirm=MessageBox.Show(totbill, MyResources.Resource.messageinfo, MessageBoxButton.OKCancel);
            if(confirm == MessageBoxResult.OK)
            {
                Tbk_total_price.Text = "";
                foreach(Tables tab in App._table)
                {
                    if (tab.tableNum == App.tableNo)
                    { 
                        tab.totalBill = 0.00;
                        tab.bookedName = "";
                    }
                }
                foreach(Orders ord in App._order.ToList())
                {
                    if(ord.tableNo == App.tableNo)
                        App._order.Remove(ord);
                }
                Lbx_order.ItemsSource=null;
            }
        }
        private void Tbx_filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = Tbx_filter.Text.ToLower();
            if (filter == "")
            {
                var sel = Cbx_MenuItems.SelectedIndex;
                switch (sel)
                {
                    case 0:
                        filter = "Drinks";
                        break;
                    case 1:
                        filter = "Mains";
                        break;
                    case 2:
                        filter = "Desserts";
                        break;
                    default:
                        filter = "Drinks";
                        break;

                }
                filter = filter.ToLower();
                var lst = (from b in App._menu where b.category.ToLower() == filter select b).ToList();
                Lbx_menu.ItemsSource = lst;     
            }
               
            else
            {
                var res = (from a in App._menu where a.name.ToLower().Contains(filter) select a).ToList();
                Lbx_menu.ItemsSource=res;
            }

        }

    }
}
