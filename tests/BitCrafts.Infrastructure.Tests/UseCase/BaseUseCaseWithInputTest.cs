using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Abstraction.UseCases;

namespace BitCrafts.Infrastructure.Tests.UseCase;

[TestClass]
public class BaseUseCaseWithInputTest
{
    private IServiceProvider _serviceProvider;
    private IBackgroundThreadDispatcher _threadDispatcher;
    private TestUseCaseWithInput _testUseCase;

    [TestInitialize]
    public void Initialize()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _threadDispatcher = Substitute.For<IBackgroundThreadDispatcher>();

        // Configure service provider to return thread dispatcher
        _serviceProvider.GetService(typeof(IBackgroundThreadDispatcher)).Returns(_threadDispatcher);

        _testUseCase = new TestUseCaseWithInput(_serviceProvider);
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
        var testCaseWithTracker = new TestUseCaseWithInputDisposeTracker(_serviceProvider);

        // Act
        testCaseWithTracker.Dispose();

        // Assert
        Assert.IsTrue(testCaseWithTracker.DisposeWasCalled);
        Assert.IsTrue(testCaseWithTracker.DisposeBoolParameterValue);
    }
}