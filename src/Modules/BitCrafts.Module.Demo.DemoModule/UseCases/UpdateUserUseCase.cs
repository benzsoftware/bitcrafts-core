using BitCrafts.Infrastructure.Abstraction.UseCases;
using BitCrafts.Module.Demo.DemoModule.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Module.Demo.DemoModule.UseCases;

public sealed class UpdateUserUseCase : BaseUseCase<User>, IUpdateUserUseCase
{
    private readonly DemoDbContext _demoDbContext;

    public UpdateUserUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _demoDbContext = serviceProvider.GetRequiredService<DemoDbContext>();
    }

    protected override async Task ExecuteCoreAsync(User input)
    {
        _demoDbContext.Update(input);
        await _demoDbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}