using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace BitCrafts.Infrastructure.Tests.UseCase;

[TestClass]
public class BaseUseCaseTests
{
    private IServiceProvider _serviceProvider;
    private IBackgroundThreadDispatcher _threadDispatcher;
    private TestUseCase _testUseCase;

    [TestInitialize]
    public void Initialize()
    {
        _serviceProvider = Substitute.For<IServiceProvider>();
        _threadDispatcher = Substitute.For<IBackgroundThreadDispatcher>();

        // Configure service provider to return thread dispatcher
        _serviceProvider.GetService(typeof(IBackgroundThreadDispatcher)).Returns(_threadDispatcher);

        _testUseCase = new TestUseCase(_serviceProvider);
    }

    [TestMethod]
    public async Task ExecuteAsync_CallsExecuteCoreAsync_ReturnsCorrectResult()
    {
        // Arrange
        string input = "test";

        // Configurer le mock du BackgroundThreadDispatcher pour exécuter la fonction fournie
        _threadDispatcher.InvokeTaskAsync(Arg.Any<Func<Task<int>>>())
            .Returns(callInfo =>
            {
                var func = callInfo.Arg<Func<Task<int>>>();
                return func(); // Exécute la fonction passée et retourne son résultat
            });

        // Act
        var result = await _testUseCase.ExecuteAsync(input);

        // Assert
        Assert.AreEqual(4, result); // "test" a une longueur de 4
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
        var testCaseWithTracker = new TestUseCaseWithDisposeTracker(_serviceProvider);

        // Act
        testCaseWithTracker.Dispose();

        // Assert
        Assert.IsTrue(testCaseWithTracker.DisposeWasCalled);
        Assert.IsTrue(testCaseWithTracker.DisposeBoolParameterValue);
    }
}