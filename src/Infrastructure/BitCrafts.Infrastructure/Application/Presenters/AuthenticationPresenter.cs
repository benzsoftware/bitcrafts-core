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
        ServiceProvider.GetRequiredService<IUiManager>().CloseWindow<IAuthenticationPresenter>();
    }

    private async void ViewOnAuthenticate(object sender, Authentication e)
    {
        var isAunthenticated = await ServiceProvider.GetRequiredService<IAuthenticationUseCase>().ExecuteAsync(e);
        if (isAunthenticated)
        {
            await ServiceProvider.GetRequiredService<IUiManager>().ShowWindowAsync<IMainPresenter>(
                new Dictionary<string, object>()
                {
                    { "WindowState", WindowState.Normal },
                    { "Width", 1600 },
                    { "Height", 900 },
                    {
                        "WindowStartupLocation", WindowStartupLocation.CenterScreen
                    }
                });
            ServiceProvider.GetRequiredService<IUiManager>().CloseWindow<IAuthenticationPresenter>();
        }

        else
        {
            View.SetAuthenticationError("Login or password is incorrect.");
        }
    }

    protected override Task OnDisAppearedAsync()
    {
        return Task.CompletedTask;
    }
}