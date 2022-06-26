namespace LostTech.Stack.Widgets.DataSources;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LostTech.Stack.Widgets.DataBinding;

public sealed class AsyncEnumerableThrottle<T> : DependencyObjectNotifyBase {
    IAsyncEnumerable<T>? source;
    public IAsyncEnumerable<T>? Source {
        get => this.source;
        set {
            this.source = value;
            this.OnPropertyChanged();
            this.OnPropertyChanged(nameof(this.Throttled));
        }
    }

    TimeSpan? throttleBy;
    public TimeSpan? ThrottleBy {
        get => this.throttleBy;
        set {
            if (value == this.throttleBy)
                return;
            this.throttleBy = value;
            this.OnPropertyChanged();
        }
    }
    public IAsyncEnumerable<T>? Throttled {
        get {
            if (this.Source is null) return null;

            return this.GetThrottled(this.Source);
        }
    }

    async IAsyncEnumerable<T> GetThrottled(IAsyncEnumerable<T> source) {
        await foreach (var item in source.ConfigureAwait(false)) {
            yield return item;

            if (this.ThrottleBy is { } throttleBy)
                await Task.Delay(throttleBy).ConfigureAwait(false);
        }
    }
}
