using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Application.Abstraction.Presenters;

public abstract class LoadablePresenter<TView, TModel> : BasePresenter<TView>
    where TView : class, ILoadableView<TModel>
    where TModel : class, IViewModel, new()
{
    protected LoadablePresenter(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Model = new TModel();
    }

    protected TModel Model { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    protected bool IsLoading { get; private set; }

    protected void UpdateModel(TModel updatedModel)
    {
        if (updatedModel is null)
            throw new ArgumentNullException(nameof(updatedModel));

        Model = updatedModel;
        if (Model is BaseViewModel baseModel)
            baseModel.ResetDirtyState();

        View.DisplayData(Model);
    }

    protected override async Task OnAppearedAsync()
    {
        await LoadDataAsync();
        View.DisplayData(Model);
    }

    protected async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            View.ShowLoading();
            Model = await FetchDataAsync();
            if (Model != null) View.DisplayData(Model);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors du chargement des données");
            View.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
            View.HideLoading();
        }
    }

    protected abstract Task<TModel> FetchDataAsync();
}