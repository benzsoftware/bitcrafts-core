using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Module.Demo.DemoModule.Presenters;

public sealed class CreateUserDialogPresenter : BasePresenter, ICreateUserDialogPresenter
{
    private readonly ICreateUserUseCase _createUserUseCase;

    private ICreateUserDialogView CreateView => View as ICreateUserDialogView;

    public CreateUserDialogPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider, typeof(CreateUserDialogPresenter))
    {
        _createUserUseCase = serviceProvider.GetRequiredService<ICreateUserUseCase>();
        CreateView.Title = "Create User";
    }

    protected override Task OnAppearedAsync()
    {
        CreateView.UserCreated += ViewOnUserCreated;
        return Task.CompletedTask;
    }

    private async void ViewOnUserCreated(object sender, User e)
    {
        await _createUserUseCase.ExecuteAsync(e);
    }

    protected override async Task OnDisappearedAsync()
    {
        CreateView.UserCreated -= ViewOnUserCreated;
        await base.OnDisappearedAsync();
    }
}