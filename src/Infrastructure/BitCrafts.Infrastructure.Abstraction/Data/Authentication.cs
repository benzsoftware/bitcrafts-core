using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BitCrafts.Infrastructure.Abstraction.Extensions;

namespace BitCrafts.Infrastructure.Abstraction.Data;

public class Authentication : BaseDataDirtyState
{
    private string _login;
    [Required] [MaxLength(100)] public string Login => _login;
    private string _password;

    [Required]
    [MaxLength(24)]
    [MinLength(8)]
    public string Password => _password;

    private string _confirmPassword;

    [Required]
    [MaxLength(24)]
    [MinLength(8)]
    public string PasswordConfirmation => _confirmPassword;

    public void SetLogin(string login)
    {
        if (login.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(login));
        if (_login != login)
        {
            _login = login;
            MarkAsDirty();
        }
    }

    public void SetPassword(string password)
    {
        if (password.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(password));
        if (_password != password)
        {
            _password = password;
            MarkAsDirty();
        }
    }

    public void SetPasswordConfirmation(string confirmationPassword)
    {
        if (confirmationPassword.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(confirmationPassword));

        if (_confirmPassword != confirmationPassword)
        {
            _confirmPassword = confirmationPassword;
            if (IsSamePassword(_password, _confirmPassword)) MarkAsDirty();
        }
    }

    private bool IsSamePassword(string password, string confirmationPassword)
    {
        return password.Equals(confirmationPassword);
    }

    public Authentication()
    {
    }

    public Authentication(string login, string password, string confirmPassword)
    {
        SetLogin(login);
        SetPassword(password);
        SetPasswordConfirmation(confirmPassword);
    }
}