using BitCrafts.Application.Abstraction.Managers;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Application.Abstraction.Presenters;

/// <summary>
///     Provides an abstract base class for presenters.
///     This class implements the IPresenter interface and provides default functionality
///     for managing the view and logging.
/// </summary>
/// <typeparam name="TView">The type of the view associated with the presenter.</typeparam>
public abstract class BasePresenter<TView> : IPresenter
    where TView : IView
{
    protected IBackgroundThreadDispatcher BackgroundThreadDispatcher { get; }

    protected IUiManager UiManager { get; }
    protected IDataValidator DataValidator { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BasePresenter{TView}" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public BasePresenter(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        BackgroundThreadDispatcher = serviceProvider.GetService<IBackgroundThreadDispatcher>();
        UiManager = serviceProvider.GetService<IUiManager>();
        Logger = ServiceProvider.GetRequiredService<ILogger<BasePresenter<TView>>>();
        View = ServiceProvider.GetRequiredService<TView>();
        DataValidator = serviceProvider.GetRequiredService<IDataValidator>();
        View.AppearedEvent += ViewOnAppearedEvent;
        View.DisappearedEvent += ViewOnDisappearedEvent;
    }

    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    ///     Gets the parameters passed to the presenter.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    protected IReadOnlyDictionary<string, object> Parameters { get; private set; }

    /// <summary>
    ///     Gets the view associated with the presenter.
    /// </summary>
    public TView View { get; }

    /// <summary>
    ///     Gets or sets the title of the view.
    /// </summary>
    public string Title { get; protected set; }

    /// <summary>
    ///     Gets the logger instance used for logging.
    /// </summary>
    protected ILogger<BasePresenter<TView>> Logger { get; }

    /// <summary>
    ///     Gets the view associated with the presenter.
    /// </summary>
    /// <returns>The view instance.</returns>
    public IView GetView()
    {
        return View;
    }

    /// <summary>
    ///     Sets the parameters for the presenter.
    ///     Parameters can be used to pass data to the presenter when it is created.
    /// </summary>
    /// <param name="parameters">A dictionary containing the parameters.</param>
    public void SetParameters(Dictionary<string, object> parameters)
    {
        Parameters = parameters;
    }

    /// <summary>
    ///     Handles the Disappeared event of the view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private async void ViewOnDisappearedEvent(object sender, EventArgs e)
    {
        Logger.LogInformation($"{GetType().Name} Disappeared");
        await OnDisappearedAsync();
    }

    /// <summary>
    ///     Handles the Appeared event of the view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private async void ViewOnAppearedEvent(object sender, EventArgs e)
    {
        Logger.LogInformation($"{GetType().Name} Appeared");
        await OnAppearedAsync();
    }

    /// <summary>
    ///     Called when the view has appeared.
    ///     Derived classes must implement this method to perform any necessary initialization.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    protected virtual Task OnAppearedAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Called when the view has disappeared.
    ///     Derived classes must implement this method to perform any necessary cleanup.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    protected virtual Task OnDisappearedAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Releases unmanaged resources used by the presenter.
    /// </summary>
    /// <param name="disposing">
    ///     True if disposing is called from the Dispose method; otherwise, false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            View.AppearedEvent -= ViewOnAppearedEvent;
            View.DisappearedEvent -= ViewOnDisappearedEvent;
            View.Dispose();
            Logger.LogInformation($"{GetType().Name} Disposed.");
        }
    }

    /// <summary>
    ///     Disposes of any resources used by the presenter.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;
        Dispose(true);
        GC.SuppressFinalize(this);
        _disposed = true;
    }

    private bool _disposed;
}