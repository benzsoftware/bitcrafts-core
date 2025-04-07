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

    static string DisappearedEventName = "DisappearedEvent";
    static string AppearedEventName = "AppearedEvent";
}