using Avalonia.Controls;

namespace BitCrafts.Application.Avalonia.Tests;

[TestClass]
public class TestClass : BaseTestClass
{
    [TestMethod]
    public async Task TestMethod1()
    {
        await Session.RunOnUIThread(() =>
        {
            var window = new Window();
            window.Show();
            window.Close();
        });
    }
}