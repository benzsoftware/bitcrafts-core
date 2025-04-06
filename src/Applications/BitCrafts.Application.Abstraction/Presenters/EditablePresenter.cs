using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Application.Abstraction.Presenters;

public abstract class EditablePresenter<TView, TModel> : LoadablePresenter<TView, TModel>
    where TView : class, IEditableView<TModel>
    where TModel : class, IViewModel, new()
{
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

    private async void OnSaveRequested(object sender, TModel model)
    {
        try
        {
            View.ShowLoading("Saving in progress ...");
            var success = await SaveDataCoreAsync(model);

            if (success)
            {
                Model = model;
                if (model is BaseViewModel baseModel) baseModel.ResetDirtyState();
            }
            else
            {
                View.ShowError("Error while saving data.");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "error while saving data.");
            View.ShowError(ex.Message);
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

    protected abstract Task<bool> SaveDataCoreAsync(TModel model);
}