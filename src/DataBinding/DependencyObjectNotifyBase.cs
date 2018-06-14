namespace LostTech.Stack.Widgets.DataBinding
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using JetBrains.Annotations;

    public class DependencyObjectNotifyBase: DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
