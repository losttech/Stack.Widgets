namespace LostTech.Stack.Widgets.DataSources
{
    using System.Windows.Input;

    public interface IRefreshable
    {
        ICommand RefreshCommand { get; }
    }
}
