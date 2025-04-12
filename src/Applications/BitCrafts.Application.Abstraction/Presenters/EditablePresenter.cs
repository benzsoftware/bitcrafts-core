using System.Text;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Application.Abstraction.Presenters;
/*
public abstract class EditablePresenter<TView, TModel> : LoadablePresenter<TView, TModel>
    where TView : class, IEditableView<TModel>
    where TModel : class, IModel, new()
{
    private CancellationTokenSource _cts = new();

    protected EditablePresenter(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    protected override void OnSubscribeEventsCore()
    {
        base.OnSubscribeEventsCore();
        EventAggregator.Subscribe(IEditableView<TModel>.CancelEventName, OnCancelRequested);
        EventAggregator.Subscribe<TModel>(IEditableView<TModel>.SaveEventName, OnSaveRequested);
    }

    protected abstract Task<bool> SaveDataCoreAsync(TModel model, CancellationToken cancellationToken = default);
    protected abstract Task CancelCoreAsync();


    private void OnCancelRequested()
    {
        CancelCoreAsync();
    }

    private void OnSaveRequested()
    {
        _ = HandleSaveRequestAsync();
    }

    private async Task HandleSaveRequestAsync()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
        var cancellationToken = _cts.Token;

        try
        {
            if (DataValidator.TryValidate(View.Model, false, out var list))
            {
                View.ShowLoading("Saving in progress ...");
                var success = await SaveDataCoreAsync(View.Model, cancellationToken);

                if (success)
                    UpdateModel(model);
                else
                    View.ShowError("Error while saving data.");
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var validationResult in list)
                    sb.AppendLine(validationResult.ErrorMessage);
                await UiManager.ShowErrorMessageAsync("Validation Error", $"Modell is not valid: \n {sb}");
                sb.Clear();
            }
        }
        catch (OperationCanceledException)
        {
            Logger.LogInformation($"{GetType().Name} save canceled.");
        }

        catch (Exception ex)
        {
            Logger.LogError(ex, "error while saving data.");
        }
        finally
        {
            View.HideLoading();
        }
    }
}*/