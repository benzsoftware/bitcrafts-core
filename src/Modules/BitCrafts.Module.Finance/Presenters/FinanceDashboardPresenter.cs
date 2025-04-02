using BitCrafts.Application.Abstraction.Application.Managers;
using BitCrafts.Application.Abstraction.Application.Presenters;
using BitCrafts.Module.Finance.Abstraction.Presenters;
using BitCrafts.Module.Finance.Abstraction.UseCases;
using BitCrafts.Module.Finance.Abstraction.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Finance.Presenters;

public sealed class FinanceDashboardPresenter : BasePresenter<IFinanceDashboardView>, IFinanceDahsboardPresenter
{
    private readonly IDeleteBankAccountUseCase _deleteBankAccountUseCase;
    private readonly IDisplayBankAccountsUseCase _displayBankAccountsUseCase;
    private readonly IDisplayUsersUseCase _displayUsersUseCase;
    private readonly IUiManager _uiManager;

    public FinanceDashboardPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _displayBankAccountsUseCase = serviceProvider.GetRequiredService<IDisplayBankAccountsUseCase>();
        _displayUsersUseCase = serviceProvider.GetRequiredService<IDisplayUsersUseCase>();
        _uiManager = serviceProvider.GetRequiredService<IUiManager>();
        _deleteBankAccountUseCase = serviceProvider.GetRequiredService<IDeleteBankAccountUseCase>();
        View.Title = "Finance";
    }

    protected override Task OnAppearedAsync()
    {
        /* var result = await _displayBankAccountsUseCase.ExecuteAsync();
         var users = await _displayUsersUseCase.ExecuteAsync();
         View.RefreshBankAccounts(result);
         View.DeleteBankAccount += ViewOnDeleteBankAccount;*/
        /* _regionManager.ShowPresenterInRegion<IDisplayBankAccountsPresenter>(0);
         _regionManager.ShowPresenterInRegion<ICreateBankAccountPresenter>(1);*/
        // _regionManager.ShowPresenterInRegion<ICreateBankAccountPresenter>(1, 1, 2);
        //_uiManager.ShowInTabControl<ICr>();
        return Task.CompletedTask;
    }


    protected override Task OnDisAppearedAsync()
    {
        return Task.CompletedTask;
    }
}