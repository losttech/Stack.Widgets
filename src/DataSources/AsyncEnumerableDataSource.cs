#nullable enable

namespace LostTech.Stack.Widgets.DataSources;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LostTech.Stack.Widgets.DataBinding;

public sealed class AsyncEnumerableDataSource<T> : DependencyObjectNotifyBase {
    IAsyncEnumerable<T>? source;
    public IAsyncEnumerable<T>? Source {
        get => this.source;
        set {
            this.source = value;
            this.OnPropertyChanged();
            this.EnumerateInternal();
        }
    }

    bool loop;
    public bool Loop {
        get => this.loop;
        set {
            if (value == this.loop)
                return;
            this.loop = value;
            this.OnPropertyChanged();
        }
    }

    T? value;
    public T? Value {
        get => this.value;
        set {
            if (value is IEquatable<T> equatable && equatable.Equals(this.value))
                return;

            this.value = value;
            this.OnPropertyChanged();
        }
    }

    Exception? error;
    /// <summary>
    /// Gets Exception, that might have occurred when the source was refreshed.
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

    async void EnumerateInternal() {
        while (this.loop) {
            try {
                await this.Enumerate();
            } catch (Exception ex) {
                this.Error = ex;
                this.Value = default;
            }
        }
    }
    async Task Enumerate() {
        if (this.source is null) throw new InvalidOperationException();
        await foreach (var val in this.source) {
            this.Value = val;
        }
    }
}
