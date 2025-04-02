using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Abstraction.Application.Views;

public interface IAuthenticationView : IView
{
    event EventHandler Cancel;
    event EventHandler<Authentication> Authenticate;
    void SetAuthenticationError(string errorMessage);

    void DisplayProgressBar();

    void HideProgressBar();
}