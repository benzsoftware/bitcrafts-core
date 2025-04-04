using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Avalonia.Controls.Views;

public abstract class BaseControl : UserControl, IView
{
    private bool _isDisposed;
    public string Title { get; set; }
    public event EventHandler DisappearedEvent;
    public event EventHandler AppearedEvent;

    protected BaseControl()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        Title = "UnTitled Control";
    }

    protected abstract void OnAppeared();
    protected abstract void OnDisappeared();


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

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
        _isDisposed = true;
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}