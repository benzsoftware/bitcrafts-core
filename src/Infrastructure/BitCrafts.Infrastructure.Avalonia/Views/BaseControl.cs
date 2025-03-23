using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Infrastructure.Abstraction.Application.Views;

namespace BitCrafts.Infrastructure.Avalonia.Views;

public abstract class BaseControl : UserControl, IView
{
    protected BaseControl()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        Title = "UnTitled Control";
        BusyText = string.Empty;
    }

    public string Title { get; set; }
    public bool IsBusy { get; private set; }
    public string BusyText { get; private set; }
    public event EventHandler DisappearedEvent;
    public event EventHandler AppearedEvent;


    public virtual void SetBusy(string message)
    {
        IsBusy = true;
        BusyText = message;
    }

    public virtual void UnsetBusy()
    {
        IsBusy = false;
        BusyText = string.Empty;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        OnDisappeared();
        DisappearedEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        OnAppeared();
        AppearedEvent?.Invoke(this, EventArgs.Empty);
    }

    protected abstract void OnAppeared();
    protected abstract void OnDisappeared();


    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }
}