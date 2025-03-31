using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Infrastructure.Abstraction.Modules;

/// <summary>
///     Defines an interface for modules.
///     Modules are self-contained units of functionality that can be added to or removed from the application.
/// </summary>
public interface IModule
{
    /// <summary>
    ///     Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Registers services with the dependency injection container.
    ///     This method is called when the module is loaded to allow the module
    ///     to register its dependencies.
    /// </summary>
    /// <param name="services">The service collection to register services with.</param>
    void RegisterServices(IServiceCollection services);

    /// <summary>
    ///     Gets the type of the presenter associated with the module.
    ///     This is used by the UI manager to display the module's UI.
    /// </summary>
    /// <returns>The type of the presenter.</returns>
    Type GetPresenterType();

    /// <summary>
    ///     Initializes the module.
    ///     This method is called when the module is loaded to allow the module
    ///     to perform any necessary initialization.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    void Initialize(IServiceProvider serviceProvider);

    void InitializeMenus(IServiceProvider serviceProvider);
}