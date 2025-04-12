using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Application.Abstraction.Models;

public class AuthenticationModel : BaseModel
{
    public string Login { get; private set; }
    public string Password { get; private set; }
    public EnvironmentConfiguration Environment { get; private set; }

    public AuthenticationModel()
    {
    }

    public AuthenticationModel(string login, string password, EnvironmentConfiguration environment)
    {
        Login = login;
        Password = password;
        Environment = environment;
    }
}