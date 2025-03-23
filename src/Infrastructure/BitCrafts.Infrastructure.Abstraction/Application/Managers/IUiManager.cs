using BitCrafts.Infrastructure.Abstraction.Application.Presenters;

namespace BitCrafts.Infrastructure.Abstraction.Application.Managers;

/// <summary>
///     Defines an interface for UI managers.
///     UI managers are responsible for managing the application's user interface,
///     including displaying windows, dialogs, and other UI elements.
/// </summary>
public interface IUiManager : IDisposable
{
    /// <summary>
    ///     Shuts down the application asynchronously.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task ShutdownAsync();

    /// <summary>
    ///     Closes a window associated with a presenter.
    /// </summary>
    /// <typeparam name="TPresenter">The type of the presenter whose window should be closed.</typeparam>
    void CloseWindow<TPresenter>() where TPresenter : class, IPresenter;

    /// <summary>
    ///     Shows a window for a presenter.
    /// </summary>
    /// <typeparam name="TPresenter">The type of the presenter whose window should be shown.</typeparam>
    /// <param name="parameters">
    ///     A dictionary containing parameters to pass to the presenter.
    ///     This can be used to provide initial data or configuration to the presenter.
    /// </param>
    void ShowWindow<TPresenter>(Dictionary<string, object> parameters = null) where TPresenter : class, IPresenter;

    /// <summary>
    ///     Shows a dialog window for a presenter asynchronously.
    /// </summary>
    /// <typeparam name="TPresenter">The type of the presenter whose dialog should be shown.</typeparam>
    /// <param name="parameters">
    ///     A dictionary containing parameters to pass to the presenter.
    ///     This can be used to provide initial data or configuration to the presenter.
    /// </param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task ShowDialogAsync<TPresenter>(Dictionary<string, object> parameters = null) where TPresenter : class, IPresenter;

    /// <summary>
    ///     Closes a dialog window associated with a presenter.
    /// </summary>
    /// <typeparam name="TPresenter">The type of the presenter whose dialog should be closed.</typeparam>
    void CloseDialog<TPresenter>() where TPresenter : class, IPresenter;

    /// <summary>
    ///     Shows a presenter's view within a tab control.
    /// </summary>
    /// <typeparam name="TPresenter">The type of the presenter whose view should be shown in the tab control.</typeparam>
    /// <param name="parameters">Optional parameters to pass to the presenter.</param>
    void ShowInTabControl<TPresenter>(Dictionary<string, object> parameters = null)
        where TPresenter : class, IPresenter;

    /// <summary>
    ///     Shows a presenter's view within a tab control using the presenter's Type.
    /// </summary>
    /// <param name="presenterType">The Type of the presenter whose view should be shown in the tab control.</param>
    /// <param name="parameters">Optional parameters to pass to the presenter.</param>
    void ShowInTabControl(Type presenterType, Dictionary<string, object> parameters = null);
}