using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Infrastructure.Abstraction.Events;

namespace BitCrafts.Application.Avalonia.Controls.Views;

public abstract class BaseView : UserControl, IView
{
    private bool _isDisposed;
    public string Title { get; set; }

    private IEventAggregator _eventAggregator;
    protected IEventAggregator EventAggregator => _eventAggregator;

    protected BaseView()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        Title = "UnTitled Control";
    }

    protected virtual void OnAppeared()
    {
    }

    protected virtual void OnDisappeared()
    {
    }


    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        OnDisappeared();
        EventAggregator?.Publish(IView.DisappearedEventName);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        OnAppeared();
        EventAggregator?.Publish(IView.AppearedEventName);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;
        Dispose(true);
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }

    public void SetEventAggregator(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }
}