using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.UserAccounts.UseCases;

public sealed class CreateUserUseCase : BaseUseCase<User>, ICreateUserUseCase
{
    private readonly UsersDbContext _usersDbContext;

    public CreateUserUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _usersDbContext = serviceProvider.GetRequiredService<UsersDbContext>();
    }

    protected override async Task ExecuteCoreAsync(User input)
    {
        await _usersDbContext.Users.AddAsync(input).ConfigureAwait(false);
        await _usersDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}