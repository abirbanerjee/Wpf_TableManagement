using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_TableManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public FlowDirection FlowDirection { get; set; }
        
        public static ObservableCollection<Tables> _table;

        public static int tableNo = 0;

        public static ObservableCollection<ResMenu> _menu;
        public static ObservableCollection<Orders> _order;
 
        public void Application_Startup(object sender, StartupEventArgs e)
        {

            _table = MyStorage.ReadXml <ObservableCollection<Tables>>("Tables.xml");
            if(_table == null)
                _table = new ObservableCollection<Tables>();
            _menu = MyStorage.ReadXml<ObservableCollection<ResMenu>>("Menu.xml");
            if (_menu == null)
                _menu = new ObservableCollection<ResMenu>();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            MyStorage.WriteXml<ObservableCollection<Tables>>(_table, "Tables.xml");
            MyStorage.WriteXml<ObservableCollection<ResMenu>>(_menu, "Menu.xml");
            MySettings.Default.Save();
        }
    }
}
