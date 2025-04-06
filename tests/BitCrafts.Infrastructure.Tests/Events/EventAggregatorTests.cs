using BitCrafts.Infrastructure.Events;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Tests.Events;

[TestClass]
[TestCategory("Events")]
public class EventAggregatorTests
{
    private ILogger<EventAggregator> _logger;
    private EventAggregator _eventAggregator;
    private const string TestEventKey = "TestEvent";

    [TestInitialize]
    public void Initialize()
    {
        _logger = Substitute.For<ILogger<EventAggregator>>();
        _eventAggregator = new EventAggregator(_logger);
    }

    [TestMethod]
    public void Subscribe_ShouldReturnSubscription()
    {
        // Act
        var subscription = _eventAggregator.Subscribe(TestEventKey, () => { });

        // Assert
        Assert.IsNotNull(subscription);
        Assert.IsInstanceOfType(subscription, typeof(IDisposable));
    }

    [TestMethod]
    public void Subscribe_MultipleHandlers_ShouldReturnDifferentSubscriptions()
    {
        // Act
        var subscription1 = _eventAggregator.Subscribe(TestEventKey, () => { });
        var subscription2 = _eventAggregator.Subscribe(TestEventKey, () => { });

        // Assert
        Assert.IsNotNull(subscription1);
        Assert.IsNotNull(subscription2);
        Assert.AreNotSame(subscription1, subscription2);
    }

    [TestMethod]
    public void Publish_ShouldInvokeSubscribedHandlers()
    {
        // Arrange
        var handlerInvoked = false;
        _eventAggregator.Subscribe(TestEventKey, () => handlerInvoked = true);

        // Act
        _eventAggregator.Publish(TestEventKey);

        // Assert
        Assert.IsTrue(handlerInvoked);
    }

    [TestMethod]
    public void Publish_ShouldInvokeMultipleSubscribedHandlers()
    {
        // Arrange
        var invocationCount = 0;
        _eventAggregator.Subscribe(TestEventKey, () => invocationCount++);
        _eventAggregator.Subscribe(TestEventKey, () => invocationCount++);
        _eventAggregator.Subscribe(TestEventKey, () => invocationCount++);

        // Act
        _eventAggregator.Publish(TestEventKey);

        // Assert
        Assert.AreEqual(3, invocationCount);
    }

    [TestMethod]
    public void Publish_AfterDisposingSubscription_ShouldNotInvokeHandler()
    {
        // Arrange
        var handlerInvoked = false;
        var subscription = _eventAggregator.Subscribe(TestEventKey, () => handlerInvoked = true);

        // Act
        subscription.Dispose();
        _eventAggregator.Publish(TestEventKey);

        // Assert
        Assert.IsFalse(handlerInvoked);
    }

    [TestMethod]
    public void Publish_HandlerThrowsException_ShouldContinueToOtherHandlers()
    {
        // Arrange
        var handler1Invoked = false;
        var handler2Invoked = false;

        _eventAggregator.Subscribe(TestEventKey, () => { throw new Exception("Test exception"); });
        _eventAggregator.Subscribe(TestEventKey, () => handler1Invoked = true);
        _eventAggregator.Subscribe(TestEventKey, () => handler2Invoked = true);

        // Act
        _eventAggregator.Publish(TestEventKey);

        // Assert
        Assert.IsTrue(handler1Invoked);
        Assert.IsTrue(handler2Invoked);
    }

    [TestMethod]
    [ExpectedException(typeof(ObjectDisposedException))]
    public void Dispose_ShouldPreventFurtherPublications()
    {
        // Arrange
        var handlerInvoked = false;
        _eventAggregator.Subscribe(TestEventKey, () => handlerInvoked = true);

        // Act
        _eventAggregator.Dispose();
        _eventAggregator.Publish(TestEventKey);

        // Assert
        Assert.IsFalse(handlerInvoked);
    }

    [TestMethod]
    public void PublishWithPayload_ShouldDeliverPayloadToHandler()
    {
        // Arrange
        var receivedPayload = string.Empty;
        var expectedPayload = "TestPayload";

        _eventAggregator.Subscribe<string>(TestEventKey, payload => receivedPayload = payload);

        // Act
        _eventAggregator.Publish(TestEventKey, expectedPayload);

        // Assert
        Assert.AreEqual(expectedPayload, receivedPayload);
    }

    [TestMethod]
    public void PublishWithPayload_AfterDisposingSubscription_ShouldNotInvokeHandler()
    {
        // Arrange
        var handlerInvoked = false;
        var subscription = _eventAggregator.Subscribe<string>(TestEventKey, _ => handlerInvoked = true);

        // Act
        subscription.Dispose();
        _eventAggregator.Publish(TestEventKey, "TestPayload");

        // Assert
        Assert.IsFalse(handlerInvoked);
    }

    [TestMethod]
    public void PublishWithPayload_HandlerThrowsException_ShouldContinueToOtherHandlers()
    {
        // Arrange
        var handler1Invoked = false;
        var handler2Invoked = false;

        _eventAggregator.Subscribe<string>(TestEventKey, _ => { throw new Exception("Test exception"); });
        _eventAggregator.Subscribe<string>(TestEventKey, _ => handler1Invoked = true);
        _eventAggregator.Subscribe<string>(TestEventKey, _ => handler2Invoked = true);

        // Act
        _eventAggregator.Publish(TestEventKey, "TestPayload");

        // Assert
        Assert.IsTrue(handler1Invoked);
        Assert.IsTrue(handler2Invoked);
    }

    [TestMethod]
    public void SubscribeAndPublish_DifferentEventKeys_ShouldNotTriggerOtherHandlers()
    {
        // Arrange
        var handler1Invoked = false;
        var handler2Invoked = false;

        _eventAggregator.Subscribe("EventKey1", () => handler1Invoked = true);
        _eventAggregator.Subscribe("EventKey2", () => handler2Invoked = true);

        // Act
        _eventAggregator.Publish("EventKey1");

        // Assert
        Assert.IsTrue(handler1Invoked);
        Assert.IsFalse(handler2Invoked);
    }

    [TestMethod]
    public void MultipleSubscriptions_ShouldAllBeDisposedIndependently()
    {
        // Arrange
        var handler1Invoked = false;
        var handler2Invoked = false;
        var handler3Invoked = false;

        _eventAggregator.Subscribe(TestEventKey, () => handler1Invoked = true);
        var subscription2 = _eventAggregator.Subscribe(TestEventKey, () => handler2Invoked = true);
        _eventAggregator.Subscribe(TestEventKey, () => handler3Invoked = true);

        // Act - dispose only subscription2
        subscription2.Dispose();
        _eventAggregator.Publish(TestEventKey);

        // Assert
        Assert.IsTrue(handler1Invoked);
        Assert.IsFalse(handler2Invoked);
        Assert.IsTrue(handler3Invoked);
    }

    [TestMethod]
    public void Subscribe_WithNullEventKey_ShouldThrowArgumentException()
    {
        // Assert
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Subscribe(null, () => { }));
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Subscribe(string.Empty, () => { }));
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Subscribe("   ", () => { }));
    }

    [TestMethod]
    public void Subscribe_WithNullHandler_ShouldThrowArgumentNullException()
    {
        // Assert
        Assert.ThrowsException<ArgumentNullException>(() => _eventAggregator.Subscribe(TestEventKey, null));
        Assert.ThrowsException<ArgumentNullException>(() => _eventAggregator.Subscribe<string>(TestEventKey, null));
    }

    [TestMethod]
    public void Publish_WithNullEventKey_ShouldThrowArgumentException()
    {
        // Assert
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Publish(null));
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Publish(string.Empty));
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Publish("   "));
    }

    [TestMethod]
    public void PublishWithPayload_WithNullEventKey_ShouldThrowArgumentException()
    {
        // Assert
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Publish(null, "payload"));
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Publish(string.Empty, "payload"));
        Assert.ThrowsException<ArgumentException>(() => _eventAggregator.Publish("   ", "payload"));
    }
}