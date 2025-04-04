using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;
using BitCrafts.Infrastructure.Abstraction.UseCases;

namespace BitCrafts.Infrastructure.Tests.UseCase;

[TestClass]
public class BaseUseCaseWithoutInputOrResultTests
{
    private IServiceProvider _serviceProvider;
    private IBackgroundThreadDispatcher _threadDispatcher;
    private TestUseCaseWithoutInputOrResult _testUseCase;

    [TestInitialize]
    public void Initialize()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _threadDispatcher = Substitute.For<IBackgroundThreadDispatcher>();

        // Configure service provider to return thread dispatcher
        _serviceProvider.GetService(typeof(IBackgroundThreadDispatcher)).Returns(_threadDispatcher);

        _testUseCase = new TestUseCaseWithoutInputOrResult(_serviceProvider);
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
        var testCaseWithTracker = new TestUseCaseWithoutInputOrResultDisposeTracker(_serviceProvider);

        // Act
        testCaseWithTracker.Dispose();

        // Assert
        Assert.IsTrue(testCaseWithTracker.DisposeWasCalled);
        Assert.IsTrue(testCaseWithTracker.DisposeBoolParameterValue);
    }
}

// Classes de test pour BaseUseCase
public class TestUseCaseWithoutInputOrResult : BaseUseCase
{
    public Func<Task> ExecuteCoreAsyncCallback { get; set; }

    public TestUseCaseWithoutInputOrResult(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public IBackgroundThreadDispatcher GetBackgroundThreadDispatcher() => BackgroundThreadDispatcher;
    public IServiceProvider GetServiceProvider() => ServiceProvider;

    protected override Task ExecuteCoreAsync()
    {
        return ExecuteCoreAsyncCallback?.Invoke() ?? Task.CompletedTask;
    }
}

public class TestUseCaseWithoutInputOrResultDisposeTracker : BaseUseCase
{
    public bool DisposeWasCalled { get; private set; }
    public bool DisposeBoolParameterValue { get; private set; }

    public TestUseCaseWithoutInputOrResultDisposeTracker(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task ExecuteCoreAsync()
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