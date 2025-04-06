using Avalonia.Headless;

namespace BitCrafts.Application.Avalonia.Tests;

public abstract class BaseTestClass
{
    protected HeadlessUnitTestSession Session { get; private set; }

    protected BaseTestClass()
    {
        Session = HeadlessUnitTestSession.StartNew(typeof(AvaloniaTestStartup));
    }

    [TestCleanup]
    public void TestCleanup()
    {
        Session.Dispose();
    }
}