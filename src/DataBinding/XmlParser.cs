namespace LostTech.Stack.Widgets.DataBinding
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Windows.Data;
    using System.Xml.Linq;
    using Newtonsoft.Json;

    public sealed class XmlParser: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return null;
            switch (value) {
            case string str:
                var document = XDocument.Parse(str);
                string json = JsonConvert.SerializeXNode(document);
                dynamic dynamicObject = JsonConvert.DeserializeObject<ExpandoObject>(json);
                if (parameter is string path) {
                    foreach (string part in path.Split('.')) {
                        dynamicObject = ((IDictionary<string, object>)dynamicObject)[part];
                    }
                }
                return dynamicObject;
            default:
                throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
