using Avalonia.Controls;
using BitCrafts.Infrastructure.Abstraction;
using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Abstraction.Application.Presenters;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Events;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.UserAccounts.Presenters;

public sealed class DisplayUserAccountsPresenter : BasePresenter<IDisplayUserAccountsView>,
    IDisplayUserAccountsPresenter
{
    private readonly IDeleteUserUseCase _deleteUserUseCase;
    private readonly IDisplayUsersUseCase _displayUsersUseCase;
    private readonly IEventAggregator _eventAggregator;
    private readonly IUpdateUserUseCase _updateUserUseCase;
    private readonly UsersDbContext _usersDbContext;
    private Guid _createUserEventId;

    public DisplayUserAccountsPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _usersDbContext = serviceProvider.GetRequiredService<UsersDbContext>();
        _displayUsersUseCase = serviceProvider.GetRequiredService<IDisplayUsersUseCase>();
        _updateUserUseCase = serviceProvider.GetRequiredService<IUpdateUserUseCase>();
        _deleteUserUseCase = serviceProvider.GetRequiredService<IDeleteUserUseCase>();
        _eventAggregator = serviceProvider.GetRequiredService<IEventAggregator>();
        View.Title = "User Accounts";
    }

    protected override async Task OnAppearedAsync()
    {
        View.CreateUser += ViewOnCreateUser;
        View.UpdateUser += ViewOnUpdateUser;
        View.DeleteUser += ViewOnDeleteUser;
        View.Refresh += ViewOnRefresh;
        _createUserEventId = _eventAggregator.Subscribe<CreateUserEvent>(OnCreateUser);
        var result = await _displayUsersUseCase.ExecuteAsync();
        View.RefreshUsers(result);
    }

    private async void ViewOnRefresh(object sender, EventArgs e)
    {
        var result = await BackgroundThreadDispatcher.InvokeTaskAsync(_displayUsersUseCase.ExecuteAsync);
        View.RefreshUsers(result);
    }

    private void OnCreateUser(CreateUserEvent obj)
    {
        View.AppendUser(obj.User);
    }

    private async void ViewOnDeleteUser(object sender, IEnumerable<User> e)
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
    }

    protected override Task OnDisAppearedAsync()
    {
        View.UpdateUser -= ViewOnUpdateUser;
        View.DeleteUser -= ViewOnDeleteUser;
        View.CreateUser -= ViewOnCreateUser;
        _eventAggregator.Unsubscribe<CreateUserEvent>(_createUserEventId);
        return Task.CompletedTask;
    }
}