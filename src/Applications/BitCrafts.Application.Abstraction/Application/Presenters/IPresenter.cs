using BitCrafts.Application.Abstraction.Application.Views;

namespace BitCrafts.Application.Abstraction.Application.Presenters;

/// <summary>
///     Defines an interface for presenters.
///     Presenters handle user interactions and update the view accordingly.
/// </summary>
public interface IPresenter : IDisposable
{
    /// <summary>
    ///     Gets the view associated with the presenter.
    /// </summary>
    /// <returns>The view instance.</returns>
    IView GetView();

    /// <summary>
    ///     Sets the parameters for the presenter.
    ///     Parameters can be used to pass data to the presenter when it is created.
    /// </summary>
    /// <param name="parameters">A dictionary containing the parameters.</param>
    void SetParameters(Dictionary<string, object> parameters);
}