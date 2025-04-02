using BitCrafts.Application.Abstraction.Application.Presenters; 
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.Presenters;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.Presenters;

public sealed class CreateBankAccountPresenter : BasePresenter<ICreateBankAccountView>, ICreateBankAccountPresenter
{
    private readonly ICreateBankAccountUseCase _createBankAccountUseCase;
    private readonly IDisplayUsersUseCase _displayUsersUseCase;

    public CreateBankAccountPresenter(IServiceProvider serviceProvider) :
        base(serviceProvider)
    {
        _createBankAccountUseCase = serviceProvider.GetRequiredService<ICreateBankAccountUseCase>();
        _displayUsersUseCase = serviceProvider.GetRequiredService<IDisplayUsersUseCase>();
        View.Title = "Create New Account";
    }

    protected override async Task OnAppearedAsync()
    {
        View.CreateBankAccount += ViewOnCreateBankAccount;
        var users = await _displayUsersUseCase.ExecuteAsync();
        View.CloseView += ViewOnCloseView;
        View.RefreshUsers(users);
    }

    private void ViewOnCloseView(object sender, EventArgs e)
    {
        //_regionManager.ClosePresenterInRegion<ICreateBankAccountPresenter>();
    }

    protected override Task OnDisAppearedAsync()
    {
        View.CreateBankAccount -= ViewOnCreateBankAccount;
        View.CloseView -= ViewOnCloseView;
        return Task.CompletedTask;
    }

    private async void ViewOnCreateBankAccount(object sender, BankAccount e)
    {
        await _createBankAccountUseCase.ExecuteAsync(e);
    }
}