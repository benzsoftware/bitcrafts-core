using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Infrastructure.Abstraction.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Application.Avalonia.Presenters;

public sealed class EnvironmentConfigurationPresenter :
    BasePresenter,
    IEnvironmentConfigurationPresenter
{
    private readonly IEnvironmentConfigurationService _environmentConfigurationService;
    private List<EnvironmentConfiguration> _environmentConfigurations;

    public EnvironmentConfigurationPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider, typeof(IEnvironmentConfigurationView))
    {
        _environmentConfigurationService = ServiceProvider.GetRequiredService<IEnvironmentConfigurationService>();
        View.Title = "Environment Configuration";
    }

    protected override async Task<IModel> LoadDataCoreAsync()
    {
        _environmentConfigurations = await _environmentConfigurationService.GetEnvironmentsAsync();
        var model = new EnvironmentConfigurationModel(_environmentConfigurations);
        return model;
    }

    protected override async Task SaveChangesCoreAsync(IModel model)
    {
        await _environmentConfigurationService.SaveEnvironmentsAsync(_environmentConfigurations)
            .ConfigureAwait(false);
    }
}