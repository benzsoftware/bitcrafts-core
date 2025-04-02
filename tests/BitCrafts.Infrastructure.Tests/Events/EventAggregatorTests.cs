using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Infrastructure.Events;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Tests.Events;

[TestClass]
[TestCategory("Events")]
public class EventAggregatorTests
{
    private ILogger<EventAggregator> _loggerMock;
    private EventAggregator _eventAggregator;

    [TestInitialize]
    public void Initialize()
    {
        _loggerMock = Substitute.For<ILogger<EventAggregator>>();
        _eventAggregator = new EventAggregator(_loggerMock);
    }

    [TestMethod]
    public void Subscribe_ShouldReturnNewGuid()
    {
        // Arrange
        Action<TestEvent> handler = _ => { };

        // Act
        var result = _eventAggregator.Subscribe(handler);

        // Assert
        Assert.AreNotEqual(Guid.Empty, result);
    }

    [TestMethod]
    public void Subscribe_MultipleHandlers_ShouldReturnDifferentGuids()
    {
        // Arrange
        Action<TestEvent> handler1 = _ => { };
        Action<TestEvent> handler2 = _ => { };

        // Act
        var guid1 = _eventAggregator.Subscribe(handler1);
        var guid2 = _eventAggregator.Subscribe(handler2);

        // Assert
        Assert.AreNotEqual(guid1, guid2);
    }

    [TestMethod]
    public void Unsubscribe_ExistingHandler_ShouldReturnTrue()
    {
        // Arrange
        Action<TestEvent> handler = _ => { };
        var handlerId = _eventAggregator.Subscribe(handler);

        // Act
        var result = _eventAggregator.Unsubscribe<TestEvent>(handlerId);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Unsubscribe_NonExistingHandler_ShouldReturnFalse()
    {
        // Arrange
        var nonExistingHandlerId = Guid.NewGuid();

        // Act
        var result = _eventAggregator.Unsubscribe<TestEvent>(nonExistingHandlerId);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Publish_ShouldInvokeSubscribedHandlers()
    {
        // Arrange
        var testEvent = new TestEvent();
        var handlerWasCalled = false;
        Action<TestEvent> handler = _ => { handlerWasCalled = true; };
        _eventAggregator.Subscribe(handler);

        // Act
        _eventAggregator.Publish(testEvent);

        // Assert
        Assert.IsTrue(handlerWasCalled);
    }

    [TestMethod]
    public void Publish_ShouldInvokeMultipleSubscribedHandlers()
    {
        // Arrange
        var testEvent = new TestEvent();
        var handler1CallCount = 0;
        var handler2CallCount = 0;

        Action<TestEvent> handler1 = _ => { handler1CallCount++; };
        Action<TestEvent> handler2 = _ => { handler2CallCount++; };

        _eventAggregator.Subscribe(handler1);
        _eventAggregator.Subscribe(handler2);

        // Act
        _eventAggregator.Publish(testEvent);

        // Assert
        Assert.AreEqual(1, handler1CallCount);
        Assert.AreEqual(1, handler2CallCount);
    }

    [TestMethod]
    public void Publish_AfterUnsubscribe_ShouldNotInvokeHandler()
    {
        // Arrange
        var testEvent = new TestEvent();
        var handlerWasCalled = false;
        Action<TestEvent> handler = _ => { handlerWasCalled = true; };
        var handlerId = _eventAggregator.Subscribe(handler);
        _eventAggregator.Unsubscribe<TestEvent>(handlerId);

        // Act
        _eventAggregator.Publish(testEvent);

        // Assert
        Assert.IsFalse(handlerWasCalled);
    }

    [TestMethod]
    public void Publish_HandlerThrowsException_ShouldContinueToOtherHandlers()
    {
        // Arrange
        var testEvent = new TestEvent();
        var handler2WasCalled = false;

        Action<TestEvent> handler1 = _ => { throw new Exception("Test exception"); };
        Action<TestEvent> handler2 = _ => { handler2WasCalled = true; };

        _eventAggregator.Subscribe(handler1);
        _eventAggregator.Subscribe(handler2);

        // Act
        _eventAggregator.Publish(testEvent);

        // Assert
        Assert.IsTrue(handler2WasCalled);
        _loggerMock.Received().Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }

    [TestMethod]
    public void Dispose_ShouldPreventFurtherPublications()
    {
        // Arrange
        var testEvent = new TestEvent();
        var handlerWasCalled = false;
        Action<TestEvent> handler = _ => { handlerWasCalled = true; };
        _eventAggregator.Subscribe(handler);

        // Act
        _eventAggregator.Dispose();
        _eventAggregator.Publish(testEvent);

        // Assert
        Assert.IsFalse(handlerWasCalled);
        _loggerMock.Received().Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Is<Exception>(ex => ex == null),
            Arg.Any<Func<object, Exception, string>>());
    }

    private class TestEvent : BaseEvent
    {
    }
}