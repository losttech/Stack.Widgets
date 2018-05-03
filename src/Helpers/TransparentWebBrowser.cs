namespace LostTech.Stack.Widgets.Helpers
{
    using System.Windows;
    using System.Windows.Controls;

    public class TransparentWebBrowser: Control
    {
        WebBrowserOverlayWindow _WebBrowserOverlayWindow;
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register(nameof(TargetElement), typeof(FrameworkElement), typeof(TransparentWebBrowser),
                new PropertyMetadata(TargetElementPropertyChanged));
        public FrameworkElement TargetElement {
            get => this.GetValue(TargetElementProperty) as FrameworkElement;
            set => this.SetValue(TargetElementProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(string), typeof(TransparentWebBrowser),
                new PropertyMetadata(SourcePropertyChanged));
        public string Source {
            get => this.GetValue(SourceProperty) as string;
            set => this.SetValue(SourceProperty, value);
        }

        private static void SourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) {
            if (sender is TransparentWebBrowser transparentWebBrowser) {
                transparentWebBrowser._WebBrowserOverlayWindow.Source = args.NewValue as string;
            }
        }

        private static void TargetElementPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) {
            if (sender is TransparentWebBrowser transparentWebBrowser) {
                transparentWebBrowser._WebBrowserOverlayWindow.TargetElement = args.NewValue as FrameworkElement;
            }
        }

        public TransparentWebBrowser() {
            this._WebBrowserOverlayWindow = new WebBrowserOverlayWindow();

            //TODO: Figure out how to automatically set the TargetElement binding...

            //var targetElementBinding = new Binding();
            //var rs = new RelativeSource();
            //rs.AncestorType = typeof(Border);
            //targetElementBinding.RelativeSource = rs;

            //_WebBrowserOverlayWindow.SetBinding(TransparentWebBrowser.TargetElementProperty, targetElementBinding);


        }

        static TransparentWebBrowser() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransparentWebBrowser), new FrameworkPropertyMetadata(typeof(TransparentWebBrowser)));
        }
    }
}
