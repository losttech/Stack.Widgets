namespace LostTech.Stack.Widgets.DataSources {
    using System;
    using System.Net.Http;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using LostTech.Stack.Widgets.DataBinding;
    using Prism.Commands;

    public sealed class WebDataSource: DependencyObjectNotifyBase, IRefreshable
    {
        string? url;
        string? content;
        Exception? error;
        DateTime contentTimestamp;
        DateTime? expiration;

        public WebDataSource() {
            this.RefreshCommand = new DelegateCommand(this.RefreshInternal);
        }

        /// <summary>
        /// Gets or sets URL to retrieve data from
        /// </summary>
        public string? Url {
            get => this.url;
            set {
                if (value == this.url)
                    return;
                this.url = value;
                this.RefreshInternal();
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// Gets text content of the response
        /// </summary>
        public string? Content {
            get => this.content;
            private set {
                if (value == this.content)
                    return;
                this.content = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// Gets Exception, that might have occurred during web request.
        /// This set to <c>null</c>, when the web server sends correct, but unsuccessful HTTP response.
        /// For that, see <see cref="Response"/>.<see cref="HttpResponseMessage.IsSuccessStatusCode">IsSuccessStatusCode</see>.
        /// </summary>
        public Exception? Error {
            get => this.error;
            private set {
                if (value == this.error) return;
                this.error = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// The time, when <see cref="Content"/> properly was last updated
        /// </summary>
        public DateTime ContentTimestamp {
            get => this.contentTimestamp;
            private set {
                if (value.Equals(this.contentTimestamp)) return;
                this.contentTimestamp = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// The time, when <see cref="Content"/> will expire, if known. If the time is known, content will be refreshed after that time.
        /// </summary>
        public DateTime? Expiration {
            get => this.expiration;
            private set {
                if (value.Equals(this.expiration)) return;
                this.expiration = value;
                this.OnPropertyChanged();
            }
        }
        /// <summary>
        /// Full response body
        /// </summary>
        public HttpResponseMessage? Response { get; private set; }
        /// <summary>
        /// Command, that initiates asynchronous <see cref="Content"/> refresh.
        /// </summary>
        public ICommand RefreshCommand { get; }

        async void RefreshInternal() {
            var client = new HttpClient();
            try {
                this.Response = await client.GetAsync(this.Url);
                this.Error = null;
            } catch (Exception e) {
                this.Content = null;
                this.ContentTimestamp = DateTime.Now;
                this.Expiration = null;
                this.Response = null;
                this.Error = e;
                return;
            }

            try {
                this.Content = this.Response.Content != null
                    ? await this.Response.Content.ReadAsStringAsync()
                    : null;
            } catch (Exception e) {
                this.Content = null;
                this.Error = e;
            }
            this.ContentTimestamp = DateTime.Now;

            if (this.Response == null) {
                this.Expiration = null;
                return;
            }

            var cacheControl = this.Response.Headers?.CacheControl;
            if (cacheControl?.MaxAge != null) {
                if (cacheControl.MaxAge.Value <= TimeSpan.Zero) {
                    this.Expiration = null;
                    return;
                }

                this.Expiration = DateTime.Now + cacheControl.MaxAge.Value;
                var timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle) {
                    Interval = cacheControl.MaxAge.Value,
                };
                void ShutdownHandler(object? sender, EventArgs e) {
                    timer?.Stop();
                    timer = null;
                }
                Application.Current.Dispatcher.ShutdownStarted += ShutdownHandler;
                timer.Tick += delegate {
                    timer?.Stop();
                    Application.Current.Dispatcher.ShutdownStarted -= ShutdownHandler;
                    timer = null;

                    this.RefreshInternal();
                };
                timer.Start();
            }
        }
    }
}
