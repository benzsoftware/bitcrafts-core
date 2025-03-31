using Avalonia.Controls;
using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Abstraction.Application.Presenters;
using BitCrafts.Infrastructure.Abstraction.Application.Views;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Infrastructure.Application.Presenters;

public class AuthenticationPresenter : BasePresenter<IAuthenticationView>, IAuthenticationPresenter
{
    private readonly IUiManager _uimanager;

    public AuthenticationPresenter(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        View.Title = "Authentication";
        _uimanager = serviceProvider.GetService<IUiManager>();
    }

    protected override Task OnAppearedAsync()
    {
        View.Authenticate += ViewOnAuthenticate;
        View.Cancel += ViewOnCancel;
        return Task.CompletedTask;
    }

    private void ViewOnCancel(object sender, EventArgs e)
    {
        _uimanager.CloseWindow<IAuthenticationPresenter>();
    }

    private async void ViewOnAuthenticate(object sender, Authentication e)
    {
        try
        {
            View.DisplayProgressBar();
            await Task.Delay(25000);
            var isAunthenticated = await ServiceProvider.GetRequiredService<IAuthenticationUseCase>().ExecuteAsync(e);
            if (isAunthenticated)
            {
                await _uimanager.ShowWindowAsync<IMainPresenter>(
                    new Dictionary<string, object>()
                    {
                        { "WindowState", WindowState.Normal },
                        { "Width", 1600 },
                        { "Height", 900 },
                        {
                            "WindowStartupLocation", WindowStartupLocation.CenterScreen
                        }
                    });
                _uimanager.CloseWindow<IAuthenticationPresenter>();
            }

            else
            {
                View.SetAuthenticationError("Login or password is incorrect.");
            }
        }
        catch (Exception exception)
        {
            View.SetAuthenticationError(exception.Message);
        }
        finally
        {
            View.HideProgressBar();
        }
    }

    protected override Task OnDisAppearedAsync()
    {
        return Task.CompletedTask;
    }
}