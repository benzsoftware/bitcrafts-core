using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.UserAccounts.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.UserAccounts.UseCases;

public sealed class DisplayUsersUseCase : BaseUseCaseWithResult<IEnumerable<User>>, IDisplayUsersUseCase
{
    private readonly UsersDbContext _usersDbContext;

    public DisplayUsersUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _usersDbContext = serviceProvider.GetRequiredService<UsersDbContext>();
    }

    protected override async Task<IEnumerable<User>> ExecuteCoreAsync()
    {
        return await _usersDbContext.Users.ToListAsync().ConfigureAwait(false);
    }
}