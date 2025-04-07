using Avalonia.Controls;
using BitCrafts.Application.Abstraction;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Application.Avalonia.Presenters;

public class AuthenticationPresenter : LoadablePresenter<IAuthenticationView, AuthenticationViewModel>,
    IAuthenticationPresenter
{
    public AuthenticationPresenter(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        View.Title = "Authentication";
    }

    protected override void OnSubscribeEventsCore()
    {
        EventAggregator.Subscribe<AuthenticationViewModel>(IAuthenticationView.AuthenticateEventName,
            ViewOnAuthenticate);
        EventAggregator.Subscribe(IAuthenticationView.CancelEventName, ViewOnCancel);
        EventAggregator.Subscribe(IAuthenticationView.ShowEnvironmentEventName, ViewOnShowEnvironment);
    }

    private void ViewOnShowEnvironment()
    {
        UiManager.ShowWindowAsync<IEnvironmentConfigurationPresenter>(new Dictionary<string, object>()
        {
            { Constants.WindowWidthParameterName, "800" },
            { Constants.WindowHeightParameterName, "600" }
        });
    }

    protected override Task<AuthenticationViewModel> FetchDataAsync()
    {
        return Task.FromResult(new AuthenticationViewModel());
    }

    private void ViewOnCancel()
    {
        UiManager.CloseWindow<IAuthenticationPresenter>();
    }

    private async void ViewOnAuthenticate(AuthenticationViewModel viewModel)
    {
        try
        {
            View.DisplayProgressBar();
            var auth = new Authentication(viewModel.Login, viewModel.Password, viewModel.Password);
            var isAunthenticated = await ServiceProvider.GetRequiredService<IAuthenticationUseCase>()
                .ExecuteAsync(auth);
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
}