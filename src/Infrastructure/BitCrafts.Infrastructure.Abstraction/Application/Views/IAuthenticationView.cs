using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Infrastructure.Abstraction.Application.Views;

public interface IAuthenticationView : IView
{
    event EventHandler Cancel;
    event EventHandler<Authentication> Authenticate;
    void SetAuthenticationError(string errorMessage);
}