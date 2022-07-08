namespace LostTech.Stack.Widgets.DataBinding;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

public class Formatter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (parameter is not string format)
            throw new ArgumentException("Parameter must be a format string");
        return string.Format(format, value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}