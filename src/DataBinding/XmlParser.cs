namespace LostTech.Stack.Widgets.DataBinding
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Xml.Linq;
    using Newtonsoft.Json;

    public sealed class XmlParser: IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return null;
            switch (value) {
            case string str:
                var document = XDocument.Parse(str);
                string json = JsonConvert.SerializeXNode(document);
                return JsonParser.GetObject(json, parameter as string);
            default:
                throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
