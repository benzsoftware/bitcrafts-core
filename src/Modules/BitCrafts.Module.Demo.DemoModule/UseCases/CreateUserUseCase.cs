using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.DemoModule.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.DemoModule.UseCases;

public sealed class CreateUserUseCase : BaseUseCase<User>, ICreateUserUseCase
{
    private readonly DemoDbContext _demoDbContext;

    public CreateUserUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _demoDbContext = serviceProvider.GetRequiredService<DemoDbContext>();
    }

    protected override async Task ExecuteCoreAsync(User input)
    {
        await _demoDbContext.Users.AddAsync(input).ConfigureAwait(false);
        await _demoDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}