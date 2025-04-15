using System.ComponentModel.DataAnnotations;
using BitCrafts.Application.Abstraction.Presenters;

namespace BitCrafts.Application.Abstraction.Managers;

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
    Task ShowWindowAsync<TPresenter>(Dictionary<string, object> parameters = null) where TPresenter : class, IPresenter;

    Task ShowWindowAsync(Type presenterType, Dictionary<string, object> parameters = null);

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
    Task CloseDialogAsync<TPresenter>() where TPresenter : class, IPresenter;

    Task CloseDialogAsync(Type presenterType);

    /// <summary>
    ///     Shows a presenter's view within a tab control.
    /// </summary>
    /// <typeparam name="TPresenter">The type of the presenter whose view should be shown in the tab control.</typeparam>
    /// <param name="parameters">Optional parameters to pass to the presenter.</param>
    Task ShowInTabControlAsync<TPresenter>(Dictionary<string, object> parameters = null)
        where TPresenter : class, IPresenter;

    /// <summary>
    ///     Shows a presenter's view within a tab control using the presenter's Type.
    /// </summary>
    /// <param name="presenterType">The Type of the presenter whose view should be shown in the tab control.</param>
    /// <param name="parameters">Optional parameters to pass to the presenter.</param>
    Task ShowInTabControlAsync(Type presenterType, Dictionary<string, object> parameters = null);

    /// <summary>
    /// Shows an error message dialog with a custom title and message.
    /// </summary>
    /// <param name="title">The title of the error dialog.</param>
    /// <param name="message">The error message to display.</param>
    /// <returns>A Task representing the asynchronous operation.  The task completes when the dialog is closed.</returns>
    Task ShowErrorMessageAsync(string title, string message);

    /// <summary>
    /// Shows an error message dialog with a custom title and the details of an exception.  The exception's message and stack trace will be included in the dialog.
    /// </summary>
    /// <param name="title">The title of the error dialog.</param>
    /// <param name="exception">The exception to display details for.</param>
    /// <returns>A Task representing the asynchronous operation. The task completes when the dialog is closed.</returns>
    Task ShowErrorMessageAsync(string title, Exception exception);

    Task ShowModelValidationErrorAsync(IReadOnlyList<ValidationResult> validationResults);
}