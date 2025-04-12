using System.Collections.ObjectModel;
using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Application.Abstraction.Models;

public class EnvironmentConfigurationModel : BaseModel
{
    public ObservableCollection<EnvironmentConfiguration> Environments { get; private set; } = new();
    public EnvironmentConfiguration SelectedEnvironment { get; private set; } = null;
    public EnvironmentConfiguration EditingEnvironment { get; private set; } = new();

    public EnvironmentConfigurationModel()
    {
    }

    public EnvironmentConfigurationModel(List<EnvironmentConfiguration> environments)
    {
        Environments = new ObservableCollection<EnvironmentConfiguration>(environments);
    }

    public void SetEnvironments(List<EnvironmentConfiguration> environments)
    {
        if (environments == null)
            throw new ArgumentNullException(nameof(environments));
        if (environments.Count == 0)
            return;
        Environments.Clear();
        Environments = new ObservableCollection<EnvironmentConfiguration>(environments);
    }

    public void SetSelectedEnvironment(EnvironmentConfiguration environment)
    {
        if (environment == null)
            throw new ArgumentNullException(nameof(environment));

        SelectedEnvironment = environment;
    }

    public void SetEditingEnvironment(EnvironmentConfiguration editingEnvironment)
    {
        if (editingEnvironment == null)
            throw new ArgumentNullException(nameof(editingEnvironment));
        EditingEnvironment = editingEnvironment;
    }
}