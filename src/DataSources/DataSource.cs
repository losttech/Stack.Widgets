namespace LostTech.Stack.Widgets.DataSources
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    public sealed class DataSource
    {
        public static Duration GetRefreshInterval(DependencyObject obj) =>
            (Duration)obj.GetValue(RefreshIntervalProperty);
        public static void SetRefreshInterval(DependencyObject obj, Duration value) =>
            obj.SetValue(RefreshIntervalProperty, value);

        public static readonly DependencyProperty RefreshIntervalProperty =
            DependencyProperty.RegisterAttached("RefreshInterval",
                typeof(Duration), typeof(DependencyObject),
                new PropertyMetadata(Duration.Forever, propertyChangedCallback: OnRefreshIntervalChanged));

        static readonly DependencyPropertyKey RefresherPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("__Refresher__",
                typeof(DispatcherTimer), typeof(DependencyObject), new PropertyMetadata(null));

        static void OnRefreshIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs change) {
            if (d is not IRefreshable refreshable)
                return;

            var timer = (DispatcherTimer)d.GetValue(RefresherPropertyKey.DependencyProperty);
            Duration value = change.NewValue is Duration duration ? duration : Duration.Forever;

            if (value.HasTimeSpan) {
                if (timer == null) {
                    timer = new DispatcherTimer {
                        Tag = refreshable,
                    };
                    timer.Tick += RefreshTimerOnTick;
                    d.SetValue(RefresherPropertyKey, timer);
                    // very inefficient, assumes infrequent changes to RefreshInterval
                    Application.Current.Dispatcher.ShutdownStarted += delegate { timer?.Stop(); };
                }

                timer.Interval = value.TimeSpan;
                timer.IsEnabled = true;
            } else if (timer != null) {
                timer.Stop();
                d.SetValue(RefresherPropertyKey.DependencyProperty, null);
            }
        }

        static void RefreshTimerOnTick(object? sender, EventArgs e) {
            var timer = (DispatcherTimer)sender!;
            var refreshable = (IRefreshable)timer.Tag;
            if (refreshable.RefreshCommand.CanExecute(null))
                refreshable.RefreshCommand.Execute(null);
        }
    }
}
