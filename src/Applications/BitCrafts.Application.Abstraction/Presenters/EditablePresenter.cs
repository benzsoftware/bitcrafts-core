using System.Text;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Application.Abstraction.Presenters;

public abstract class EditablePresenter<TView, TModel> : LoadablePresenter<TView, TModel>
    where TView : class, IEditableView<TModel>
    where TModel : class, IViewModel, new()
{
    private CancellationTokenSource _cts = new();

    protected EditablePresenter(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override async Task OnAppearedAsync()
    {
        View.SaveRequested += OnSaveRequested;
        View.CancelRequested += OnCancelRequested;
        await Task.CompletedTask;
    }

    protected override async Task OnDisappearedAsync()
    {
        View.SaveRequested -= OnSaveRequested;
        View.CancelRequested -= OnCancelRequested;
        await Task.CompletedTask;
    }

    private void OnSaveRequested(object sender, TModel model)
    {
        _ = HandleSaveRequestAsync(model);
    }

    private async Task HandleSaveRequestAsync(TModel model)
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
        var cancellationToken = _cts.Token;

        try
        {
            if (DataValidator.TryValidate(model, false, out var list))
            {
                View.ShowLoading("Saving in progress ...");
                var success = await SaveDataCoreAsync(model, cancellationToken);

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

    private void OnCancelRequested(object sender, EventArgs e)
    {
        if (Model != null) View.DisplayData(Model);
    }

    protected abstract Task<bool> SaveDataCoreAsync(TModel model, CancellationToken cancellationToken = default);
}