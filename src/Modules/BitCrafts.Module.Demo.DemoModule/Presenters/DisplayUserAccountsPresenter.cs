using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Models;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.DemoModule.Presenters;

public sealed class DisplayUserAccountsPresenter : BasePresenter, IDisplayUserAccountsPresenter
{
    private readonly IDisplayUsersUseCase _displayUsersUseCase;
    private IDisplayUserAccountsView DisplayView => View as IDisplayUserAccountsView;

    public DisplayUserAccountsPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        DisplayView.Title = "User Accounts";
        _displayUsersUseCase = serviceProvider.GetRequiredService<IDisplayUsersUseCase>();
    }

    /* private async void ViewOnRefresh(object sender, EventArgs e)
     {
         var result = await BackgroundThreadDispatcher.InvokeTaskAsync(_displayUsersUseCase.ExecuteAsync);
         View.RefreshUsers(result);
     }*/

    /* private void OnCreateUser(CreateUserEvent obj)
     {
         View.AppendUser(obj.User);
     }*/

    /*private async void ViewOnDeleteUser(object sender, IEnumerable<User> e)
    {
        await BackgroundThreadDispatcher.InvokeTaskAsync(async () => await _deleteUserUseCase.ExecuteAsync(e));
    }

    private async void ViewOnUpdateUser(object sender, User e)
    {
        await BackgroundThreadDispatcher.InvokeTaskAsync(async () => await _updateUserUseCase.ExecuteAsync(e));
    }

    private async void ViewOnCreateUser(object sender, EventArgs e)
    {
        await UiManager.ShowDialogAsync<ICreateUserDialogPresenter>(new Dictionary<string, object>()
        {
            { Constants.WindowWidthParameterName, 500 },
            { Constants.WindowHeightParameterName, 400 },
            { Constants.WindowSystemDecorationParameterName, SystemDecorations.Full }
        });
    }*/
/*
    protected override async Task<DisplayAccountsModel> LoadDataCoreAsync()
    {

    }
    */

    protected override async Task<IModel> LoadDataCoreAsync()
    {
        var model = new DisplayAccountsModel();
        var result = await _displayUsersUseCase.ExecuteAsync();
        model.Users = new List<User>(result);
        return model;
    }
}