using System.ComponentModel.DataAnnotations;
using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Application.Abstraction.Models;

public class AuthenticationModel : BaseModel
{
    [Required] public string Login { get; private set; }
    [Required] public string Password { get; private set; }
    [Required] public EnvironmentConfiguration Environment { get; private set; }

    public AuthenticationModel(string login, string password, EnvironmentConfiguration environment)
    {
        SetLogin(login);
        SetPassword(password);
        SetEnvironment(environment);
    }

    public void SetLogin(string newLogin)
    {
        if (Login != newLogin)
        {
            Login = newLogin;
            MarkAsDirty();
        }
    }

    public void SetPassword(string newPassword)
    {
        if (Password != newPassword)
        {
            Password = newPassword;
            MarkAsDirty();
        }
    }

    public void SetEnvironment(EnvironmentConfiguration newEnvironment)
    {
        if (!Equals(Environment, newEnvironment))
        {
            Environment = newEnvironment;
            MarkAsDirty();
        }
    }
}