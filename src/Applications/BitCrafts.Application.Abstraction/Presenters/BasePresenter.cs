using System.ComponentModel.DataAnnotations;
using BitCrafts.Application.Abstraction.Events;
using BitCrafts.Application.Abstraction.Managers;
using BitCrafts.Application.Abstraction.Models;
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
    protected bool CanSaveOnDisappear { get; set; } = false;

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
    }

    public void SetView(Type viewType)
    {
        _view = (IView)ServiceProvider.GetRequiredService(viewType);
        if (_view is IEventAware eventAwareView)
        {
            eventAwareView.SetEventAggregator(EventAggregator);
            OnSubscribeEvents();
        }
    }

    protected IServiceProvider ServiceProvider { get; }


    protected IReadOnlyDictionary<string, object> Parameters { get; private set; }


    public string Title { get; protected set; }


    protected ILogger<BasePresenter> Logger { get; }

    public void SetParameters(Dictionary<string, object> parameters)
    {
        Parameters = parameters;
    }

    private async void ViewOnDisappearedEvent()
    {
        Logger.LogInformation($"{GetType().Name} Disappeared");
        await OnDisappearedAsync();
    }

    private async void ViewOnAppearedEvent()
    {
        Logger.LogInformation($"{GetType().Name} Appeared");
        await OnAppearedAsync();
    }

    private void OnSubscribeEvents()
    {
        EventAggregator.Subscribe(ViewEvents.Base.AppearedEventName, ViewOnAppearedEvent);
        EventAggregator.Subscribe(ViewEvents.Base.DisappearedEventName, ViewOnDisappearedEvent);
        EventAggregator.Subscribe<List<ValidationResult>>(ViewEvents.Base.ErrorUpdateModelEventName,
            OnModelValidationError);

        OnSubscribeEventsCore();
    }

    private void OnModelValidationError(List<ValidationResult> validationResults)
    {
        if (validationResults != null && validationResults.Count > 0)
        {
            UiManager.ShowModelValidationErrorAsync(validationResults);
        }
    }

    protected virtual void OnSubscribeEventsCore()
    {
    }

    protected virtual async Task OnAppearedAsync()
    {
        await LoadDataAsync();
    }

    protected virtual async Task OnDisappearedAsync()
    {
        if (CanSaveOnDisappear)
        {
            View.UpdateModelFromInputs();
            await SaveChangesAsync();
        }
    }

    private async Task SaveChangesAsync()
    {
        try
        {
            View.SetBusy(true);
            if (View.Model.IsDirty)
            {
                if (View.DataValidator.TryValidate(View.Model, false, out var list))
                {
                    await SaveChangesCoreAsync(View.Model);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error saving changes");
            await UiManager.ShowErrorMessageAsync("Error saving changes", ex.Message)
                .ConfigureAwait(false);
        }
        finally
        {
            View.SetBusy(false);
        }
    }

    protected virtual async Task SaveChangesCoreAsync(IModel model)
    {
        await Task.CompletedTask;
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

    protected async Task UpdateModel(IModel model)
    {
        if (model != null)
        {
            if (!View.SetModel(model, out var validationResults))
            {
                await UiManager.ShowModelValidationErrorAsync(validationResults).ConfigureAwait(false);
            }
        }
    }

    private async Task LoadDataAsync()
    {
        try
        {
            View.SetBusy(true);
            var model = await LoadDataCoreAsync().ConfigureAwait(false);
            await UpdateModel(model).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading data");
            await UiManager.ShowErrorMessageAsync("Error loading data", ex.Message)
                .ConfigureAwait(false);
        }
        finally
        {
            View.SetBusy(false);
        }
    }

    protected virtual Task<IModel> LoadDataCoreAsync()
    {
        return Task.FromResult(default(IModel));
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