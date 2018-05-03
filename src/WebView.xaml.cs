namespace LostTech.Stack.Widgets
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Interaction logic for WebView.xaml
    /// </summary>
    public partial class WebView : UserControl
    {
        public WebView() {
            this.InitializeComponent();

            //this.Browser.Navigated += this.BrowserOnNavigated;
        }

        void BrowserOnNavigated(object sender, NavigationEventArgs e) {
            if (this.URL?.Equals(e.Uri) == true)
                return;

            this.URL = e.Uri;
        }

        #region URL
        public Uri URL {
            get { return (Uri)this.GetValue(URLProperty); }
            set { this.SetValue(URLProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty URLProperty =
            DependencyProperty.Register(nameof(URL), typeof(Uri), typeof(WebView), 
                new PropertyMetadata(defaultValue: new Uri("about:blank")
                    //, propertyChangedCallback: UrlChanged
                    ),
                validateValueCallback: ValidateUrl);

        static bool ValidateUrl(object value) {
            if (value is Uri uri) {
                string scheme = uri.Scheme?.ToLowerInvariant();
                return scheme == "http" || scheme == "https"
                    || scheme == "about" && uri.AbsoluteUri == "about:blank";
            }

            return false;
        }

        //static void UrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        //    var self = (WebView)d;
        //    var destination = e.NewValue as Uri ?? new Uri("about:blank");
        //    self.Browser.Navigate(destination);
        //}
        #endregion URL
    }
}
