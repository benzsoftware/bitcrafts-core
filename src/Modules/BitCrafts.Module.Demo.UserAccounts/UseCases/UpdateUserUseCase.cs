using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.UserAccounts.UseCases;

public sealed class UpdateUserUseCase : BaseUseCase<User>, IUpdateUserUseCase
{
    private readonly UsersDbContext _usersDbContext;

    public UpdateUserUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _usersDbContext = serviceProvider.GetRequiredService<UsersDbContext>();
    }

    protected override async Task ExecuteCoreAsync(User input)
    {
        _usersDbContext.Update(input);
        await _usersDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}