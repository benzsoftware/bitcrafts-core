using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Application.Abstraction.Presenters;

public abstract class LoadablePresenter : BasePresenter, ILoadablePresenter
{
    private ILoadableView LoadableView => View as ILoadableView;

    protected LoadablePresenter(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override async Task OnAppearedAsync()
    {
        await LoadDataAsync();
    }

    protected async Task LoadDataAsync()
    {
        try
        {
            LoadableView.SetBusy(true);
            var model = await LoadDataCoreAsync();
            if (model != null) View.SetModel(model);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors du chargement des données");
            LoadableView.ShowLoadingMessage(ex.Message);
        }
        finally
        {
            LoadableView.SetBusy(false);
        }
    }

    protected abstract Task<IModel> LoadDataCoreAsync();
}