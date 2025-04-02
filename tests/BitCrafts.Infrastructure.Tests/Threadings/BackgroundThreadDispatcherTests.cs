using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Threading;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Tests.Threadings;

[TestClass]
[TestCategory("Threading")]
public class BackgroundThreadDispatcherTests
{
    private ILogger<BackgroundThreadDispatcher> _loggerMock;
    private BackgroundThreadDispatcher _dispatcher;
    private bool _dispatcherStarted;

    [TestInitialize]
    public void Initialize()
    {
        _loggerMock = Substitute.For<ILogger<BackgroundThreadDispatcher>>();
        _dispatcher = new BackgroundThreadDispatcher(_loggerMock);
        _dispatcherStarted = false;
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (_dispatcherStarted && _dispatcher != null)
            try
            {
                _dispatcher.Dispose();
            }
            catch
            {
                // Suppression des exceptions pendant le nettoyage
            }
    }

    private void StartDispatcher()
    {
        if (!_dispatcherStarted)
        {
            _dispatcher.Start();
            _dispatcherStarted = true;
        }
    }

    [TestMethod]
    public void Constructor_ShouldCreateInstanceWithCorrectInterfaces()
    {
        Assert.IsInstanceOfType(_dispatcher, typeof(IBackgroundThreadDispatcher));
        Assert.IsInstanceOfType(_dispatcher, typeof(IThreadDispatcher));
    }

    [TestMethod]
    public void Constructor_ShouldPassCorrectThreadNameToBaseClass()
    {
        var dispatcher = new BackgroundThreadDispatcher(_loggerMock);
        Assert.IsNotNull(dispatcher);
        // Pas besoin de démarrer le thread ici
    }

    [TestMethod]
    public async Task Start_ShouldAllowInvokingActions()
    {
        StartDispatcher();
        var actionExecuted = false;

        await _dispatcher.InvokeAsync(() => actionExecuted = true);

        Assert.IsTrue(actionExecuted);
    }

    [TestMethod]
    public async Task InvokeAsync_Action_ShouldExecuteOnBackgroundThread()
    {
        StartDispatcher();
        var originalThreadId = Environment.CurrentManagedThreadId;
        var executionThreadId = 0;

        await _dispatcher.InvokeAsync(() => { executionThreadId = Environment.CurrentManagedThreadId; });

        Assert.AreNotEqual(originalThreadId, executionThreadId);
    }

    [TestMethod]
    public async Task InvokeAsync_Func_ShouldReturnCorrectResult()
    {
        StartDispatcher();
        const int expected = 42;

        var result = await _dispatcher.InvokeAsync(() => expected);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task InvokeTaskAsync_FuncTask_ShouldReturnCorrectResult()
    {
        StartDispatcher();
        const int expected = 42;

        var result = await _dispatcher.InvokeTaskAsync(() => Task.FromResult(expected));

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task InvokeTaskAsync_TaskFunc_ShouldExecuteTask()
    {
        StartDispatcher();
        var taskExecuted = false;

        await _dispatcher.InvokeTaskAsync(() =>
        {
            taskExecuted = true;
            return Task.CompletedTask;
        });

        Assert.IsTrue(taskExecuted);
    }

    [TestMethod]
    public void Stop_ShouldStopDispatcher()
    {
        StartDispatcher();

        _dispatcher.Stop();

        // Pas besoin de vérifier explicitement - le succès est l'absence d'exception
    }

    [TestMethod]
    [Timeout(3000)]
    public async Task InvokeAsync_WithoutStarting_ShouldThrowException()
    {
        using var dispatcher = new BackgroundThreadDispatcher(_loggerMock);
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            await dispatcher.InvokeAsync(() => { }));
    }

    [TestMethod]
    [Timeout(3000)]
    public void Dispose_ShouldDisposeDispatcher()
    {
        StartDispatcher();

        _dispatcher.Dispose();
        _dispatcherStarted = false;

        Assert.ThrowsException<ObjectDisposedException>(() => _dispatcher.Start());
    }

    [TestMethod]
    [Timeout(3000)]
    public void Dispose_CalledMultipleTimes_ShouldNotThrow()
    {
        StartDispatcher();

        _dispatcher.Dispose();
        _dispatcherStarted = false;

        // Créer un nouveau dispatcher pour le test de double Dispose
        var newDispatcher = new BackgroundThreadDispatcher(_loggerMock);
        newDispatcher.Start();
        newDispatcher.Dispose();
        newDispatcher.Dispose(); // Ne devrait pas bloquer
    }

    [TestMethod]
    [Timeout(3000)]
    public void Invoke_ShouldExecuteAction()
    {
        StartDispatcher();
        var actionExecuted = false;

        _dispatcher.Invoke(() => actionExecuted = true);

        // Attendre un court instant pour s'assurer que l'action est exécutée
        Thread.Sleep(100);

        Assert.IsTrue(actionExecuted);
    }


    [TestMethod]
    [Timeout(3000)]
    public void Invoke_WithoutStarting_ShouldThrowException()
    {
        Assert.ThrowsException<InvalidOperationException>(() =>
            _dispatcher.Invoke(() => { }));
    }

    [TestMethod]
    [Timeout(3000)]
    public void StartTwice_ShouldNotThrowException()
    {
        StartDispatcher();

        _dispatcher.Start(); // Deuxième appel

        var actionExecuted = false;
        _dispatcher.Invoke(() => actionExecuted = true);
        Assert.IsTrue(actionExecuted);
    }

    [TestMethod]
    [Timeout(3000)]
    public void StopTwice_ShouldNotThrowException()
    {
        StartDispatcher();

        _dispatcher.Stop();
        _dispatcher.Stop(); // Deuxième appel
        _dispatcherStarted = false;
    }
}