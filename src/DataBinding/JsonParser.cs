namespace LostTech.Stack.Widgets.DataBinding {
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;
    using Newtonsoft.Json;

    public sealed class JsonParser : IValueConverter {
        public static object? GetObject(string json, string? path) {
            dynamic? dynamicObject = JsonConvert.DeserializeObject<ExpandoObject>(json);
            if (path is not null) {
                var currentPath = new StringBuilder();
                foreach (string part in path.Split('.')) {
                    currentPath.Append(part);
                    if (dynamicObject is null)
                        throw new NullReferenceException($"Object '{currentPath}' is null");

                    if (part.StartsWith('[') && part.EndsWith(']')
                        && int.TryParse(part[1..^1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int index)) {
                        dynamicObject = ((IEnumerable<object>)dynamicObject).ElementAt(index);
                    } else {
                        dynamicObject = ((IDictionary<string, object>)dynamicObject)[part];
                    }
                    currentPath.Append('.');
                }
            }
            return dynamicObject;
        }
        
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value switch {
                null => null,
                string json => GetObject(json, parameter as string),
                _ => throw new NotSupportedException(),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
