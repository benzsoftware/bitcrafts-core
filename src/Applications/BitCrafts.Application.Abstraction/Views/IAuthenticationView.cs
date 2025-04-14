using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Abstraction.Views;

public interface IAuthenticationView : IView
{
    void SetAuthenticationError(string errorMessage);
}