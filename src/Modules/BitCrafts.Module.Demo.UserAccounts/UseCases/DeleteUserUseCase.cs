using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.UserAccounts.UseCases;

public sealed class DeleteUserUseCase : BaseUseCase<IEnumerable<User>>, IDeleteUserUseCase
{
    private readonly UsersDbContext _usersDbContext;

    public DeleteUserUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _usersDbContext = serviceProvider.GetRequiredService<UsersDbContext>();
    }

    protected override async Task ExecuteCoreAsync(IEnumerable<User> input)
    {
        _usersDbContext.RemoveRange(input);
        await _usersDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}