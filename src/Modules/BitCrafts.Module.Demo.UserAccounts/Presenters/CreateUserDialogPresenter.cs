using BitCrafts.Infrastructure.Abstraction.Application.Presenters;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Events;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Presenters;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Module.Demo.UserAccounts.Presenters;

public sealed class CreateUserDialogPresenter : BasePresenter<ICreateUserDialogView>, ICreateUserDialogPresenter
{
    private readonly ICreateUserUseCase _createUserUseCase;
    private readonly IEventAggregator _eventAggregator;

    public CreateUserDialogPresenter(IServiceProvider serviceProvider, ICreateUserUseCase createUserUseCase,
        ILogger<BasePresenter<ICreateUserDialogView>> logger, IEventAggregator eventAggregator)
        : base(serviceProvider)
    {
        _createUserUseCase = createUserUseCase;
        _eventAggregator = eventAggregator;
        View.Title = "Create User";
    }

    protected override Task OnAppearedAsync()
    {
        View.UserCreated += ViewOnUserCreated;
        return Task.CompletedTask;
    }

    private async void ViewOnUserCreated(object sender, User e)
    {
        await _createUserUseCase.ExecuteAsync(e);
        _eventAggregator.Publish(new CreateUserEvent(e));
    }

    protected override Task OnDisAppearedAsync()
    {
        View.UserCreated -= ViewOnUserCreated;
        return Task.CompletedTask;
    }
}