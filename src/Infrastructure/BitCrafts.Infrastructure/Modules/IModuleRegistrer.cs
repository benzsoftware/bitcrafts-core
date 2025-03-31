using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Infrastructure.Modules;

/// <summary>
///     Defines an interface for module registrars.
///     Module registrars are responsible for discovering and registering modules
///     with the application.
/// </summary>
public interface IModuleRegistrer : IDisposable
{
    /// <summary>
    ///     Registers modules with the dependency injection container.
    ///     This method is called to allow the registrar to discover and register modules.
    /// </summary>
    /// <param name="services">The service collection to register modules with.</param>
    void RegisterModules(IServiceCollection services);
}