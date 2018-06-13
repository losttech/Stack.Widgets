namespace LostTech.Stack.Widgets
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Interaction logic for WebView.xaml
    /// </summary>
    public partial class WebView : UserControl, IDisposable
    {
        public WebView() {
            this.InitializeComponent();

            this.Loaded += (sender, args) => {
                var parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                    parentWindow.Closed += (o, eventArgs) => { this.Dispose(); };
            };
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
        #endregion URL

        public void Dispose() {
            this.Browser.Dispose();
        }

        void WebView_OnLoaded(object sender, RoutedEventArgs e) {
            this.FixLayoutBugWithDPI();
        }

        // see https://github.com/Microsoft/WindowsCommunityToolkit/issues/2076
        async void FixLayoutBugWithDPI() {
            await Task.Yield();

            var width = this.GetValue(FrameworkElement.WidthProperty);
            this.Width = 600.0;
            await Task.Yield();
            this.SetValue(FrameworkElement.WidthProperty, width);
        }
    }
}
