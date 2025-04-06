using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Abstraction.UseCases;

namespace BitCrafts.Infrastructure.Tests.UseCase;

[TestClass]
public class BaseUseCaseWithResultOnlyTests
{
    private IServiceProvider _serviceProvider;
    private IBackgroundThreadDispatcher _threadDispatcher;
    private TestUseCaseWithResultOnly _testUseCase;

    [TestInitialize]
    public void Initialize()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _threadDispatcher = Substitute.For<IBackgroundThreadDispatcher>();

        // Configure service provider to return thread dispatcher
        _serviceProvider.GetService(typeof(IBackgroundThreadDispatcher)).Returns(_threadDispatcher);

        _testUseCase = new TestUseCaseWithResultOnly(_serviceProvider);
    }

    [TestMethod]
    public async Task ExecuteAsync_CallsExecuteCoreAsync_ReturnsCorrectResult()
    {
        // Arrange
        string expectedResult = "result";

        // Configure la méthode du mock pour appeler la fonction passée
        _threadDispatcher.InvokeTaskAsync(Arg.Any<Func<Task<string>>>())
            .Returns(callInfo =>
            {
                var func = callInfo.Arg<Func<Task<string>>>();
                return func(); // Exécute la fonction passée et retourne son résultat
            });

        _testUseCase.ResultToReturn = expectedResult;

        // Act
        var result = await _testUseCase.ExecuteAsync();

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public void Constructor_InitializesServiceProvider()
    {
        // Assert
        Assert.IsNotNull(_testUseCase.GetServiceProvider());
        Assert.AreSame(_serviceProvider, _testUseCase.GetServiceProvider());
    }

    [TestMethod]
    public void Constructor_InitializesBackgroundThreadDispatcher()
    {
        // Assert
        Assert.IsNotNull(_testUseCase.GetBackgroundThreadDispatcher());
        Assert.AreSame(_threadDispatcher, _testUseCase.GetBackgroundThreadDispatcher());
    }

    [TestMethod]
    public void Dispose_CallsDisposeMethod()
    {
        // Arrange
        var testCaseWithTracker = new TestUseCaseWithResultOnlyDisposeTracker(_serviceProvider);

        // Act
        testCaseWithTracker.Dispose();

        // Assert
        Assert.IsTrue(testCaseWithTracker.DisposeWasCalled);
        Assert.IsTrue(testCaseWithTracker.DisposeBoolParameterValue);
    }
}

// Classes de test pour BaseUseCaseWithResult<TResult>
public class TestUseCaseWithResultOnly : BaseUseCaseWithResult<string>
{
    public string ResultToReturn { get; set; } = "default";

    public TestUseCaseWithResultOnly(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public IBackgroundThreadDispatcher GetBackgroundThreadDispatcher() => BackgroundThreadDispatcher;
    public IServiceProvider GetServiceProvider() => ServiceProvider;

    protected override Task<string> ExecuteCoreAsync()
    {
        return Task.FromResult(ResultToReturn);
    }
}

public class TestUseCaseWithResultOnlyDisposeTracker : BaseUseCaseWithResult<string>
{
    public bool DisposeWasCalled { get; private set; }
    public bool DisposeBoolParameterValue { get; private set; }

    public TestUseCaseWithResultOnlyDisposeTracker(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task<string> ExecuteCoreAsync()
    {
        return Task.FromResult("test");
    }

    protected override void Dispose(bool disposing)
    {
        DisposeWasCalled = true;
        DisposeBoolParameterValue = disposing;
        base.Dispose(disposing);
    }
}