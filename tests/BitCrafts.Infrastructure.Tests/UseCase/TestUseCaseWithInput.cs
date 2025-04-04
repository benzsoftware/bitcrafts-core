using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Abstraction.UseCases;

namespace BitCrafts.Infrastructure.Tests.UseCase;

// Classes de test pour BaseUseCase<TInput>
public class TestUseCaseWithInput : BaseUseCase<string>
{
    public string LastInput { get; private set; }
    public Func<string, Task> ExecuteCoreAsyncCallback { get; set; }

    public TestUseCaseWithInput(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public IBackgroundThreadDispatcher GetBackgroundThreadDispatcher() => BackgroundThreadDispatcher;
    public IServiceProvider GetServiceProvider() => ServiceProvider;

    protected override Task ExecuteCoreAsync(string input)
    {
        LastInput = input;
        return ExecuteCoreAsyncCallback?.Invoke(input) ?? Task.CompletedTask;
    }
}