using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Presenters;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Application.Avalonia.Presenters;

public sealed class EnvironmentConfigurationPresenter :
    EditablePresenter<IEnvironmentConfigurationView, EnvironmentConfigurationViewModel>,
    IEnvironmentConfigurationPresenter
{
    public EnvironmentConfigurationPresenter(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        View.Title = "Environment Configuration";
    }

    protected override Task<EnvironmentConfigurationViewModel> FetchDataAsync()
    {
        return Task.FromResult<EnvironmentConfigurationViewModel>(new EnvironmentConfigurationViewModel()
        {
            Environments =
            {
                new EnvironmentConfiguration()
                {
                    Description = "Local Production Environment",
                    Name = "Local Production Environment",
                    Id = "Local Production Environment",
                    Type = EnvironmentType.Production,
                    ConnectionString = "Data Source=Databases/internal-production.db",
                    DatabaseProvider = DatabaseProviderType.SqlLite,
                    IsDefault = true
                }
            }
        });
    }

    protected override Task<bool> SaveDataCoreAsync(EnvironmentConfigurationViewModel model,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
        //throw new NotImplementedException();
    }
}