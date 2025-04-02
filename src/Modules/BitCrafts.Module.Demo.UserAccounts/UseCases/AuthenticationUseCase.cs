using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.UseCases;

namespace BitCrafts.Module.Demo.UserAccounts.UseCases;

public sealed class AuthenticationUseCase : BaseUseCase<Authentication, bool>, IAuthenticationUseCase
{
    public AuthenticationUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override async Task<bool> ExecuteCoreAsync(Authentication input)
    {
        await Task.Delay(3000);
        return await Task.FromResult(true);
    }
}