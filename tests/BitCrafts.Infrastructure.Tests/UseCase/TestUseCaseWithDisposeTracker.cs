using BitCrafts.Infrastructure.Abstraction.UseCases;

namespace BitCrafts.Infrastructure.Tests.UseCase;

public class TestUseCaseWithDisposeTracker : BaseUseCase<string, int>
{
    public bool DisposeWasCalled { get; private set; }
    public bool DisposeBoolParameterValue { get; private set; }

    public TestUseCaseWithDisposeTracker(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task<int> ExecuteCoreAsync(string input)
    {
        return Task.FromResult(input.Length);
    }

    protected override void Dispose(bool disposing)
    {
        DisposeWasCalled = true;
        DisposeBoolParameterValue = disposing;
        base.Dispose(disposing);
    }
}

public class TestUseCaseWithInputDisposeTracker : BaseUseCase<string>
{
    public bool DisposeWasCalled { get; private set; }
    public bool DisposeBoolParameterValue { get; private set; }

    public TestUseCaseWithInputDisposeTracker(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task ExecuteCoreAsync(string input)
    {
        return Task.CompletedTask;
    }

    protected override void Dispose(bool disposing)
    {
        DisposeWasCalled = true;
        DisposeBoolParameterValue = disposing;
        base.Dispose(disposing);
    }
}