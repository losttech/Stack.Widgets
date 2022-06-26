namespace LostTech.Stack.Widgets.DataBinding
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;
    using HtmlAgilityPack;

    public sealed class HtmlToTextConverter: IValueConverter {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return null;

            if (value is not string str)
                throw new ArgumentException("Only string values are supported");

            return this.ConvertHtml(str)?.Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();


        public string Convert(string path) {
            var doc = new HtmlDocument();
            doc.Load(path);

            var sw = new StringWriter();
            this.ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public string ConvertHtml(string html) {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var sw = new StringWriter();
            this.ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        private void ConvertContentTo(HtmlNode node, TextWriter outText) {
            foreach (HtmlNode subnode in node.ChildNodes) {
                this.ConvertTo(subnode, outText);
            }
        }

        public void ConvertTo(HtmlNode node, TextWriter outText) {
            string html;
            switch (node.NodeType) {
            case HtmlNodeType.Comment:
                // don't output comments
                break;

            case HtmlNodeType.Document:
                this.ConvertContentTo(node, outText);
                break;

            case HtmlNodeType.Text:
                // script and style must not be output
                string parentName = node.ParentNode.Name;
                if ((parentName == "script") || (parentName == "style"))
                    break;

                // get text
                html = ((HtmlTextNode)node).Text;

                // is it in fact a special closing node output as text?
                if (HtmlNode.IsOverlappedClosingElement(html))
                    break;

                // check the text is meaningful and not a bunch of whitespaces
                if (html.Trim().Length > 0) {
                    outText.Write(HtmlEntity.DeEntitize(html));
                }
                break;

            case HtmlNodeType.Element:
                switch (node.Name) {
                case "p":
                    // treat paragraphs as crlf
                    outText.Write("\r\n\r\n");
                    break;
                }

                if (node.HasChildNodes) {
                    this.ConvertContentTo(node, outText);
                }
                break;
            }
        }
    }
}
