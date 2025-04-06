namespace BitCrafts.Application.Abstraction.Views;

/// <summary>
///     Defines an interface for views.
///     Views are responsible for displaying data to the user and handling user input.
/// </summary>
public interface IView : IViewEventAware, IDisposable
{
    /// <summary>
    ///     Gets or sets the title of the view.
    /// </summary>
    string Title { get; set; }

    /// <summary>
    ///     Occurs when the view has disappeared (e.g., when a window is closed or a control is unloaded).
    /// </summary>
    event EventHandler DisappearedEvent;

    /// <summary>
    ///     Occurs when the view has appeared (e.g., when a window is shown or a control is loaded).
    /// </summary>
    event EventHandler AppearedEvent;
}