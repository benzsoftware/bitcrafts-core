using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Application.Abstraction.Models;

public class EnvironmentConfigurationViewModel : BaseViewModel
{
    public List<EnvironmentConfiguration> Environments { get; private set; } = new();
    public EnvironmentConfiguration SelectedEnvironment { get; private set; } = null;
    public EnvironmentConfiguration EditingEnvironment { get; private set; } = new();

    public EnvironmentConfigurationViewModel()
    {
    }

    public EnvironmentConfigurationViewModel(List<EnvironmentConfiguration> environments)
    {
        Environments = environments;
    }

    public void SetEnvironments(List<EnvironmentConfiguration> environments)
    {
        if (environments == null)
            throw new ArgumentNullException(nameof(environments));
        if (environments.Count == 0)
            return;
        Environments.Clear();
        Environments.AddRange(environments);
    }

    public void SetSelectedEnvironment(EnvironmentConfiguration environment)
    {
        if (environment == null)
            throw new ArgumentNullException(nameof(environment));

        SelectedEnvironment = environment;
    }
}