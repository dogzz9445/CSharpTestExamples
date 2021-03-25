using DAssist.Model;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Forms;

namespace DAssist.Converter
{
    public class KeysToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Keys keyValue = (Keys)value;
            if (Keys.NumPad9 >= keyValue && keyValue >= Keys.NumPad0)
            {
                int numpadValue = (int)keyValue - 96;
                return numpadValue.ToString();
            }

            return Enum.GetName(typeof(Keys), keyValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class KeyModifierToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = Enum.GetName(typeof(KeyModifier), (KeyModifier)value);

            return strValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
