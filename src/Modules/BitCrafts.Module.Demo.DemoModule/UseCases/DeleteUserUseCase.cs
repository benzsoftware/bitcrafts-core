using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.DemoModule.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.DemoModule.UseCases;

public sealed class DeleteUserUseCase : BaseUseCase<IEnumerable<User>>, IDeleteUserUseCase
{
    private readonly DemoDbContext _demoDbContext;

    public DeleteUserUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _demoDbContext = serviceProvider.GetRequiredService<DemoDbContext>();
    }

    protected override async Task ExecuteCoreAsync(IEnumerable<User> input)
    {
        _demoDbContext.RemoveRange(input);
        await _demoDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}