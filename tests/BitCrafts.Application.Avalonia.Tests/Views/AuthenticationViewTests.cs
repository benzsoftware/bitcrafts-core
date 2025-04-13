using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Events;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Application.Avalonia.Views;
using BitCrafts.Infrastructure.Abstraction.Events;
using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Application.Avalonia.Tests.Views;

[TestClass]
public class AuthenticationViewTests : BaseTestClass
{
    [TestMethod]
    public async Task AuthenticationView_AuthenticateButton_PublishesAuthenticationEvent()
    {
        await Session.RunOnUIThread(() =>
        {
            var eventAggregator = Substitute.For<IEventAggregator>();
            var view = new AuthenticationView();
            view.SetEventAggregator(eventAggregator);

            view.FindControl<TextBox>("LoginTextBox").Text = "user";
            view.FindControl<TextBox>("PasswordTextBox").Text = "user";

            var environnement = new EnvironmentConfiguration();
            view.FindControl<ComboBox>("EnvironmentComboBox").SelectedItem = environnement;

            view.FindControl<Button>("AuthenticateButton").RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            eventAggregator.Received(1).Publish(
                ViewEvents.Authentication.AuthenticateEventName,
                Arg.Any<AuthenticationModel>());
        });
    }
}