using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace MyFirstWindowsApp.Converters
{
    public class BooleanToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        #region Implemntation of IValueConverter
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return false;

            return (bool) value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value != null && value.Equals(TrueValue);
        }
        #endregion
    }

    public class BooleanToStringConverter : BooleanToValueConverter<string>
    {

    }
}
