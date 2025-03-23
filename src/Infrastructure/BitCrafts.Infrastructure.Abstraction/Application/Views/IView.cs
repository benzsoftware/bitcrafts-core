namespace BitCrafts.Infrastructure.Abstraction.Application.Views;

/// <summary>
///     Defines an interface for views.
///     Views are responsible for displaying data to the user and handling user input.
/// </summary>
public interface IView : IDisposable
{
    /// <summary>
    ///     Gets or sets the title of the view.
    /// </summary>
    string Title { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the view is busy.
    /// </summary>
    bool IsBusy { get; }

    /// <summary>
    ///     Gets the message to display when the view is busy.
    /// </summary>
    string BusyText { get; }

    /// <summary>
    ///     Sets the view to a busy state and displays a message.
    /// </summary>
    void SetBusy(string text);

    /// <summary>
    ///     Sets the view to an idle state.
    /// </summary>
    void UnsetBusy();

    /// <summary>
    ///     Occurs when the view has disappeared (e.g., when a window is closed or a control is unloaded).
    /// </summary>
    event EventHandler DisappearedEvent;

    /// <summary>
    ///     Occurs when the view has appeared (e.g., when a window is shown or a control is loaded).
    /// </summary>
    event EventHandler AppearedEvent;
}