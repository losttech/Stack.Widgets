namespace LostTech.Stack.Widgets.Helpers
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for WebBrowserOverlayWindow.xaml
    /// </summary>
    public partial class WebBrowserOverlayWindow : Window
    {
        public WebBrowserOverlayWindow()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register(nameof(TargetElement), typeof(FrameworkElement), typeof(WebBrowserOverlayWindow),
                new PropertyMetadata(TargetElementChanged));
        public FrameworkElement TargetElement {
            get => this.GetValue(TargetElementProperty) as FrameworkElement;
            set => this.SetValue(TargetElementProperty, value);
        }


        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(string), typeof(WebBrowserOverlayWindow),
                new PropertyMetadata(SourcePropertyChanged));
        public string Source {
            get => this.GetValue(SourceProperty) as string;
            set => this.SetValue(SourceProperty, value);
        }
        private static void SourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) {
            var webBrowserOverlayWindow = sender as WebBrowserOverlayWindow;

            if (webBrowserOverlayWindow != null) {
                webBrowserOverlayWindow.Browser.Navigate(args.NewValue as string);
            }
        }

        private static void TargetElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) {
            var oldTargetElement = args.OldValue as FrameworkElement;
            var self = sender as WebBrowserOverlayWindow;
            if (self == null)
                return;

            self.TargetElement.LayoutUpdated += self.PositionAndResize;
            self.TargetElement.IsVisibleChanged += self.OnTargetElementOnIsVisibleChanged;

            if (oldTargetElement != null) {
                oldTargetElement.LayoutUpdated -= self.PositionAndResize;
                oldTargetElement.IsVisibleChanged -= self.OnTargetElementOnIsVisibleChanged;
            }

            var mainWindow = GetWindow(self.TargetElement);
            if (mainWindow != null) {
                self.SetOwner(mainWindow);

                self.PositionAndResize(sender, new EventArgs());

                if (self.TargetElement.IsVisible && self.Owner.IsVisible) {
                    self.Show();
                }
            }
        }

        void OnTargetElementOnIsVisibleChanged(object x, DependencyPropertyChangedEventArgs y) {
            var mainWindow = GetWindow(this.TargetElement);
            if (mainWindow != null) {
                this.SetOwner(mainWindow);

                this.PositionAndResize(this, new EventArgs());

                if (this.TargetElement.IsVisible && this.Owner.IsVisible) {
                    this.Show();
                }
            }

            if (this.TargetElement.IsVisible && this.Owner.IsVisible) {
                this.Show();
            } else {
                this.Hide();
            }
        }

        void SetOwner(Window newOwner) {
            if (newOwner == null)
                throw new ArgumentNullException(nameof(newOwner));
            if (this.Owner != null)
                this.Owner.LocationChanged -= this.PositionAndResize;
            this.Owner = newOwner;
            this.Owner.LocationChanged += this.PositionAndResize;
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);

            if (this.Owner != null)
                this.Owner.LocationChanged -= this.PositionAndResize;
            if (this.TargetElement != null)
                this.TargetElement.LayoutUpdated -= this.PositionAndResize;
        }

        private void PositionAndResize(object sender, EventArgs e) {
            var mainWindow = GetWindow(this.TargetElement);
            if (mainWindow != null && mainWindow != this.Owner) {
                this.Owner = mainWindow;

                if (this.TargetElement.IsVisible && this.Owner.IsVisible) {
                    this.Show();
                }
            }

            if (this.TargetElement != null && this.TargetElement.IsVisible) {
                var point = this.TargetElement.PointToScreen(new Point());
                this.Left = point.X;
                this.Top = point.Y;

                this.Height = this.TargetElement.ActualHeight;
                this.Width = this.TargetElement.ActualWidth;
            }
        }
    }
}
