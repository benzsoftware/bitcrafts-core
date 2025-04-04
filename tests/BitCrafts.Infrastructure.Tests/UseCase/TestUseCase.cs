using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Abstraction.UseCases;

namespace BitCrafts.Infrastructure.Tests.UseCase;

public class TestUseCase : BaseUseCase<string, int>
{
    public TestUseCase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public IBackgroundThreadDispatcher GetBackgroundThreadDispatcher() => BackgroundThreadDispatcher;
    public IServiceProvider GetServiceProvider() => ServiceProvider;

    protected override Task<int> ExecuteCoreAsync(string input)
    {
        return Task.FromResult(input?.Length ?? 0);
    }
}