using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Abstraction.Application.Presenters;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Events;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Module.Demo.UserAccounts.Presenters;

public sealed class DisplayUserAccountsPresenter : BasePresenter<IDisplayUserAccountsView>,
    IDisplayUserAccountsPresenter
{
    private readonly IDeleteUserUseCase _deleteUserUseCase;
    private readonly IDisplayUsersUseCase _displayUsersUseCase;
    private readonly IEventAggregator _eventAggregator;
    private readonly IUiManager _uiManager;
    private readonly IUpdateUserUseCase _updateUserUseCase;
    private readonly UsersDbContext _usersDbContext;

    public DisplayUserAccountsPresenter(IServiceProvider serviceProvider, IDisplayUserAccountsView view,
        ILogger<BasePresenter<IDisplayUserAccountsView>> logger, UsersDbContext usersDbContext,
        IDisplayUsersUseCase displayUsersUseCase, IUpdateUserUseCase updateUserUseCase, IUiManager uiManager,
        IDeleteUserUseCase deleteUserUseCase, IEventAggregator eventAggregator)
        : base(serviceProvider)
    {
        _usersDbContext = usersDbContext;
        _displayUsersUseCase = displayUsersUseCase;
        _updateUserUseCase = updateUserUseCase;
        _uiManager = uiManager;
        _deleteUserUseCase = deleteUserUseCase;
        _eventAggregator = eventAggregator;
        View.Title = "User Accounts";
    }

    protected override async Task OnAppearedAsync()
    {
        View.CreateUser += ViewOnCreateUser;
        View.UpdateUser += ViewOnUpdateUser;
        View.DeleteUser += ViewOnDeleteUser;
        _eventAggregator.Subscribe<CreateUserEvent>(OnCreateUser);
        var result = await _displayUsersUseCase.ExecuteAsync();
        View.RefreshUsers(result);
    }

    private void OnCreateUser(CreateUserEvent obj)
    {
        View.AppendUser(obj.User);
    }

    private async void ViewOnDeleteUser(object sender, IEnumerable<User> e)
    {
        await _deleteUserUseCase.ExecuteAsync(e);
    }

    private async void ViewOnUpdateUser(object sender, User e)
    {
        await _updateUserUseCase.ExecuteAsync(e);
    }

    private async void ViewOnCreateUser(object sender, EventArgs e)
    {
        await _uiManager.ShowDialogAsync<ICreateUserDialogPresenter>();
    }

    protected override Task OnDisAppearedAsync()
    {
        View.UpdateUser -= ViewOnUpdateUser;
        View.DeleteUser -= ViewOnDeleteUser;
        View.CreateUser -= ViewOnCreateUser;
        _eventAggregator.Unsubscribe<CreateUserEvent>(OnCreateUser);
        return Task.CompletedTask;
    }
}