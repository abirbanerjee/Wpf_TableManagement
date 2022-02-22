using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Wpf_TableManagement
{
    public class String2FlowDirection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Contains("ToRight"))
                return FlowDirection.LeftToRight;
            else
                return FlowDirection.RightToLeft;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class status2color : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(value)
            {
                case 0:
                    return $"Blue";
                case 1:
                    return $"Green";
                default:
                    return $"Green";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class String2ImagePath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return $"/Images/Image.jpg";
            }
            else
            {
                if (File.Exists(value.ToString()))
                {
                    return value;
                }
                else
                {
                    return $"/Images/Image.jpg";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Date2String : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tabdate = (DateTime)value;
            var res = DateTime.Compare(tabdate, DateTime.Now);
            if (res < 0)
            {
                return "";
            }
            else 
                return(tabdate.ToString("dd/MM HH:mm"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Int2String : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((int)value)
            {
                case 0: return "Female";
                case 1: return "Male";
                case 2: return "Diverse";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)value)
            {
                case "Female": return 0;
                case "Male": return 1;
                case "Diverse": return 2;
            }
            return null;
        }
    }
}
