namespace LostTech.Stack.Widgets.DataSources;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

using LostTech.Stack.Widgets.DataBinding;

using Prism.Commands;

public abstract class AsyncDataSourceBase<T> : DependencyObjectNotifyBase, IRefreshable {
    T? value;
    Exception? error;
    DateTime timestamp;

    protected AsyncDataSourceBase() {
        this.RefreshCommand = new DelegateCommand(this.RefreshInternal);
    }

    bool autoRefresh = false;
    /// <summary>
    /// Gets or sets whether the data should automatically refresh when the source is changed.
    /// </summary>
    [DefaultValue(false)]
    public bool AutoRefresh {
        get => this.autoRefresh;
        set {
            if (value == this.autoRefresh)
                return;
            this.autoRefresh = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the content of the response
    /// </summary>
    public T? Value {
        get => this.value;
        protected set {
            if (value is IEquatable<T> equatable && equatable.Equals(this.value))
                return;
            
            this.value = value;
            this.OnPropertyChanged();
        }
    }
    /// <summary>
    /// Gets Exception, that might have occurred when the source was refreshed.
    /// This set to <c>null</c>, when the web server sends correct, but unsuccessful HTTP response.
    /// For that, see <see cref="Response"/>.<see cref="HttpResponseMessage.IsSuccessStatusCode">IsSuccessStatusCode</see>.
    /// </summary>
    public Exception? Error {
        get => this.error;
        protected set {
            if (value == this.error) return;
            this.error = value;
            this.OnPropertyChanged();
        }
    }
    /// <summary>
    /// The time, when <see cref="Value"/> properly was last updated
    /// </summary>
    public DateTime ValueTimestamp {
        get => this.timestamp;
        protected set {
            if (value.Equals(this.timestamp)) return;
            this.timestamp = value;
            this.OnPropertyChanged();
        }
    }
    /// <summary>
    /// Command, that initiates asynchronous <see cref="Value"/> refresh.
    /// </summary>
    public ICommand RefreshCommand { get; }

    protected void DoAutoRefresh() {
        if (this.autoRefresh)
            this.RefreshInternal();
    }

    private async void RefreshInternal() {
        try {
            this.Value = await this.RefreshValueAsync();
        } catch (Exception ex) {
            this.Value = default;
            this.Error = ex;
        }

        this.ValueTimestamp = DateTime.Now;
    }

    protected abstract Task<T> RefreshValueAsync();
}
