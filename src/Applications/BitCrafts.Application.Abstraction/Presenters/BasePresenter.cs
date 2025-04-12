using BitCrafts.Application.Abstraction.Events;
using BitCrafts.Application.Abstraction.Managers;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Application.Abstraction.Presenters;

public abstract class BasePresenter : IPresenter

{
    protected IBackgroundThreadDispatcher BackgroundThreadDispatcher { get; }
    protected IEventAggregator EventAggregator { get; private set; }
    protected IUiManager UiManager { get; }
    protected IDataValidator DataValidator { get; }

    private IView _view;
    public IView View => _view;

    public BasePresenter(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        BackgroundThreadDispatcher = serviceProvider.GetService<IBackgroundThreadDispatcher>();
        UiManager = serviceProvider.GetService<IUiManager>();
        Logger = ServiceProvider.GetRequiredService<ILogger<BasePresenter>>();
        DataValidator = serviceProvider.GetRequiredService<IDataValidator>();
        EventAggregator = serviceProvider.GetRequiredService<IEventAggregator>();
        if (View is IEventAware eventAwareView)
        {
            eventAwareView.SetEventAggregator(EventAggregator);
            OnSubscribeEvents();
        }
    }

    public void SetView(IView view)
    {
        _view = (IView)ServiceProvider.GetRequiredService(view.GetType());
    }

    protected IServiceProvider ServiceProvider { get; }


    protected IReadOnlyDictionary<string, object> Parameters { get; private set; }


    public string Title { get; protected set; }


    protected ILogger<BasePresenter> Logger { get; }

    public void SetParameters(Dictionary<string, object> parameters)
    {
        Parameters = parameters;
    }

    private void ViewOnDisappearedEvent()
    {
        Logger.LogInformation($"{GetType().Name} Disappeared");
        _ = OnDisappearedAsync();
    }

    private void ViewOnAppearedEvent()
    {
        Logger.LogInformation($"{GetType().Name} Appeared");
        _ = OnAppearedAsync();
    }

    private void OnSubscribeEvents()
    {
        EventAggregator.Subscribe(ViewEvents.Base.AppearedEventName, ViewOnAppearedEvent);
        EventAggregator.Subscribe(ViewEvents.Base.DisappearedEventName, ViewOnDisappearedEvent);
        OnSubscribeEventsCore();
    }

    protected virtual void OnSubscribeEventsCore()
    {
    }

    protected virtual Task OnAppearedAsync()
    {
        OnSubscribeEvents();
        return Task.CompletedTask;
    }

    protected virtual Task OnDisappearedAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            EventAggregator.Dispose();
            View.Dispose();
            Logger.LogInformation($"{GetType().Name} Disposed.");
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        Dispose(true);
        GC.SuppressFinalize(this);
        _disposed = true;
    }

    private bool _disposed;
}