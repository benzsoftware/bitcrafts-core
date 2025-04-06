using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.DemoModule.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.DemoModule.UseCases;

public sealed class DisplayUsersUseCase : BaseUseCaseWithResult<IEnumerable<User>>, IDisplayUsersUseCase
{
    private readonly DemoDbContext _demoDbContext;

    public DisplayUsersUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _demoDbContext = serviceProvider.GetRequiredService<DemoDbContext>();
    }

    protected override async Task<IEnumerable<User>> ExecuteCoreAsync()
    {
        return await _demoDbContext.Users.ToListAsync().ConfigureAwait(false);
    }
}