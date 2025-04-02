using Avalonia.Controls;
using BitCrafts.Application.Abstraction;
using BitCrafts.Application.Abstraction.Application.Presenters;
using BitCrafts.Application.Abstraction.Application.Views;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Application.Avalonia.Presenters;

public class AuthenticationPresenter : BasePresenter<IAuthenticationView>, IAuthenticationPresenter
{
    
    public AuthenticationPresenter(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        View.Title = "Authentication";
    }

    protected override Task OnAppearedAsync()
    {
        View.Authenticate += ViewOnAuthenticate;
        View.Cancel += ViewOnCancel;
        return Task.CompletedTask;
    }

    private void ViewOnCancel(object sender, EventArgs e)
    {
        UiManager.CloseWindow<IAuthenticationPresenter>();
    }

    private async void ViewOnAuthenticate(object sender, Authentication e)
    {
        try
        {
            View.DisplayProgressBar();
            var isAunthenticated = await ServiceProvider.GetRequiredService<IAuthenticationUseCase>().ExecuteAsync(e);
            if (isAunthenticated)
            {
                await UiManager.ShowWindowAsync<IMainPresenter>(
                    new Dictionary<string, object>()
                    {
                        { Constants.WindowStateParameterName, WindowState.Normal },
                        { Constants.WindowWidthParameterName, 1600 },
                        { Constants.WindowHeightParameterName, 900 },
                        {
                            Constants.WindowStartupLocationParameterName, WindowStartupLocation.CenterScreen
                        }
                    });
                UiManager.CloseWindow<IAuthenticationPresenter>();
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
        View.Authenticate -= ViewOnAuthenticate;
        View.Cancel -= ViewOnCancel;

        return Task.CompletedTask;
    }
}