namespace LostTech.Stack.Widgets.DataSources
{
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using LostTech.Stack.Widgets.DataBinding;
    using Prism.Commands;

    public class RefreshableDataSource: DependencyObjectNotifyBase, IRefreshable {
        public object Source {
            get => this.GetValue(SourceProperty);
            set => this.SetValue(SourceProperty, value);
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(RefreshableDataSource), new PropertyMetadata(null));

        public RefreshableDataSource() {
            this.RefreshCommand = new DelegateCommand(this.Refresh);
        }

        public ICommand RefreshCommand { get; }

        void Refresh() {
            var bindingExpression = BindingOperations.GetBindingExpression(this, SourceProperty);
            bindingExpression?.UpdateTarget();
        }
    }
}
