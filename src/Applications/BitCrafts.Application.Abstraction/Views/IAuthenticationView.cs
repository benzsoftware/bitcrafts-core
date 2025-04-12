using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Abstraction.Views;

public interface IAuthenticationView : IView
{
    static string CancelEventName = "Authentication.Cancel";
    static string AuthenticateEventName = "Authentication.Authenticate";
    static string ShowEnvironmentEventName = "Authentication.ShowEnvironment";
    void SetAuthenticationError(string errorMessage);

    void DisplayProgressBar();

    void HideProgressBar();
}