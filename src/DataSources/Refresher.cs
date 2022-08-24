namespace LostTech.Stack.Widgets.DataSources;

using System;
using System.Windows.Threading;

class Refresher : IDisposable {
    readonly EventHandler weakDispose;
    public DispatcherTimer Timer { get; }
    public IRefreshable Refreshable { get; }

    public Refresher(IRefreshable refreshable) {
        this.Refreshable = refreshable;
        this.Timer = new DispatcherTimer { Tag = this };
        var self = new WeakReference(this);
        this.weakDispose = (_, __) => (self.Target as IDisposable)?.Dispose();
        this.Timer.Dispatcher.ShutdownStarted += this.weakDispose;
    }

    public void Dispose() {
        this.Timer.Dispatcher.ShutdownStarted -= this.weakDispose;
        this.Timer.Stop();
    }
}
