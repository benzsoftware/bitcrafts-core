using Avalonia.Controls;
using BitCrafts.Application.Abstraction;
using BitCrafts.Application.Abstraction.Events;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.Services;
using BitCrafts.Infrastructure.Abstraction.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Application.Avalonia.Presenters;

public class AuthenticationPresenter : BasePresenter,
    IAuthenticationPresenter
{
    private IAuthenticationView AuthView => (IAuthenticationView)View;
    private readonly IEnvironmentConfigurationService _environmentConfigurationService;

    public AuthenticationPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider, typeof(IAuthenticationView))
    {
        _environmentConfigurationService = serviceProvider.GetRequiredService<IEnvironmentConfigurationService>();
        View.Title = "Authentication";
    }


    protected override void OnSubscribeEventsCore()
    {
        EventAggregator.Subscribe(ViewEvents.Base.CloseWindowEventName, ViewOnCancel);
        EventAggregator.Subscribe(ViewEvents.Authentication.AuthenticateEventName, ViewOnAuthenticate);
        EventAggregator.Subscribe(ViewEvents.Authentication.ShowEnvironmentsEventName, ViewOnShowEnvironment);
    }

    protected override async Task<IModel> LoadDataCoreAsync()
    {
        var environments = await _environmentConfigurationService.GetEnvironmentsAsync();
        return new AuthenticationModel(string.Empty, string.Empty, environments.FirstOrDefault());
    }

    private void ViewOnShowEnvironment()
    {
        UiManager.ShowWindowAsync<IEnvironmentConfigurationPresenter>(new Dictionary<string, object>()
        {
            { Constants.WindowWidthParameterName, "800" },
            { Constants.WindowHeightParameterName, "600" }
        });
    }

    private void ViewOnCancel()
    {
        UiManager.CloseWindow<IAuthenticationPresenter>();
    }

    private async void ViewOnAuthenticate()
    {
        try
        {
            AuthView.SetBusy(true, "Authenticating...");
            var model = View.GetModel() as AuthenticationModel;
            if (model == null)
                return;
            var auth = new Authentication(model.Login, model.Password, model.Password);
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
                AuthView.SetAuthenticationError("Login or password is incorrect.");
            }
        }
        catch (Exception exception)
        {
            AuthView.SetAuthenticationError(exception.Message);
        }
        finally
        {
            AuthView.SetBusy(false);
        }
    }
}