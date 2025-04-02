using BitCrafts.Application.Abstraction.Application.Presenters;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.Presenters;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.Presenters;

public sealed class DisplayBankAccountsPresenter : BasePresenter<IDisplayBankAccountsView>,
    IDisplayBankAccountsPresenter
{
    private readonly IDeleteBankAccountUseCase _deleteBankAccountUseCase;
    private readonly IDisplayBankAccountsUseCase _displayBankAccountsUseCase;

    public DisplayBankAccountsPresenter(IServiceProvider serviceProvider) :
        base(serviceProvider)
    {
        _displayBankAccountsUseCase = serviceProvider.GetRequiredService<IDisplayBankAccountsUseCase>();
        _deleteBankAccountUseCase = serviceProvider.GetRequiredService<IDeleteBankAccountUseCase>();
        View.Title = "Bank Accounts";
    }

    protected override async Task OnAppearedAsync()
    {
        var result = await _displayBankAccountsUseCase.ExecuteAsync();
        View.RefreshBankAccounts(result);
        View.DeleteBankAccount += ViewOnDeleteBankAccount;
    }

    private async void ViewOnDeleteBankAccount(object sender, BankAccount e)
    {
        await _deleteBankAccountUseCase.ExecuteAsync(new List<BankAccount> { e });
    }

    protected override Task OnDisAppearedAsync()
    {
        View.DeleteBankAccount -= ViewOnDeleteBankAccount;
        return Task.CompletedTask;
    }
}