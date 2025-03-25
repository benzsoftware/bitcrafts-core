using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using BitCrafts.Infrastructure.Abstraction.Application.Presenters;
using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Application.Dialogs;
using BitCrafts.Infrastructure.Application.Presenters;
using BitCrafts.Infrastructure.Application.Views;
using BitCrafts.Infrastructure.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Application.Managers;

public sealed class AvaloniaUiManager : IUiManager
{
    private readonly ILogger<AvaloniaUiManager> _logger;
    private readonly Dictionary<IPresenter, DefaultTabItem> _presenterToTabItemMap = new();
    private readonly Dictionary<IPresenter, Window> _presenterToWindowMap = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly Stack<Window> _windowStack = new();
    private Window _activeWindow;
    private IClassicDesktopStyleApplicationLifetime _applicationLifetime;
    private Window _rootWindow;
    private TabControl _tabControl;
    private bool _useAuthentication;

    public AvaloniaUiManager(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<AvaloniaUiManager>>();
        _serviceProvider = serviceProvider;
    }

    public Task ShowErrorMessageAsync(string title, string message)
    {
        var dialog = new ErrorMessageDialog();
        dialog.SetMessage(title, message);
        return dialog.ShowDialog(_activeWindow);
    }

    public Task ShowErrorMessageAsync(string title, Exception exception)
    {
        var dialog = new ErrorMessageDialog();
        dialog.SetMessage(title, exception.Message);
        return dialog.ShowDialog(_activeWindow);
    }

    public void ShowInTabControl<TPresenter>(Dictionary<string, object> parameters) where TPresenter : class, IPresenter
    {
        var presenterType = typeof(TPresenter);
        ShowInTabControl(presenterType, parameters);
    }

    public void ShowInTabControl(Type presenterType, Dictionary<string, object> parameters)
    {
        var existingTabItem = HasExistingPresenterInTabItems(presenterType);
        if (existingTabItem != null)
        {
            HandleExistingTabItem(existingTabItem);
            return;
        }

        var presenter = GetPresenterFromType(presenterType);
        presenter.SetParameters(parameters);

        var view = presenter.GetView();
        if (view is not UserControl)
            throw new InvalidOperationException("The view associated with the presenter is not a UserControl.");
        var tabItem = new DefaultTabItem();
        _presenterToTabItemMap[presenter] = tabItem;
        _tabControl.Items.Add(tabItem);
        _tabControl.SelectedItem = tabItem;
        tabItem.SetTitle(presenter.GetView().Title);
        tabItem.Close += (_, _) =>
        {
            _tabControl.Items.Remove(tabItem);
            _presenterToTabItemMap.Remove(presenter);
            presenter.Dispose();
        };
        tabItem.SetContent(presenter.GetView() as UserControl);
    }

    public Task ShutdownAsync()
    {
        _logger.LogInformation("Shutting down application...");
        if (_activeWindow != null) _activeWindow.Close();

        if (_rootWindow != null) _rootWindow.Close();

        return Task.CompletedTask;
    }

    public void ShowWindow<TPresenter>(Dictionary<string, object> parameters) where TPresenter : class, IPresenter
    {
        var presenterType = typeof(TPresenter);
        if (HasExistingPresenterInWindow(presenterType)) return;

        var presenter = GetPresenterFromType(presenterType);
        if (presenter != null && _presenterToWindowMap.ContainsKey(presenter))
        {
            HandleExistingWindow(_presenterToWindowMap[presenter]);
            return;
        }

        if (presenter != null)
        {
            presenter.SetParameters(parameters);
            var view = presenter.GetView();
            if (view is not UserControl)
                throw new InvalidOperationException("The view associated with the presenter is not a UserControl.");
            var window = CreateWindow(presenter, parameters);
            AddWindowToCollections(presenter, window);

            if (_rootWindow == null)
            {
                _rootWindow = window;
                _activeWindow = window;
                _applicationLifetime.MainWindow = _rootWindow;
                window.Show();
            }
            else
            {
                window.Show();
                _activeWindow = window;
            }
        }
    }

    public void CloseWindow<TPresenter>() where TPresenter : class, IPresenter
    {
        var presenter = GetPresenterFromAnyWindow<TPresenter>();
        if (presenter != null && _presenterToWindowMap.TryGetValue(presenter, out var window))
        {
            if (_rootWindow == window && _activeWindow != window)
            {
                _rootWindow = _activeWindow;
                _applicationLifetime.MainWindow = _rootWindow;
            }

            window.Close();
        }
    }

    public async Task ShowDialogAsync<TPresenter>(Dictionary<string, object> parameters)
        where TPresenter : class, IPresenter
    {
        var presenterType = typeof(TPresenter);
        var presenter = GetPresenterFromType(presenterType);

        if (presenter == null) return;

        presenter.SetParameters(parameters);
        var view = presenter.GetView();
        if (view is not UserControl)
            throw new InvalidOperationException("The view associated with the presenter is not a UserControl.");

        var dialog = CreateDialog(presenter, parameters);
        AddWindowToCollections(presenter, dialog);
        _activeWindow = dialog;
        await dialog.ShowDialog(_rootWindow);
    }

    public void CloseDialog<TPresenter>() where TPresenter : class, IPresenter
    {
        var presenter = GetPresenterFromAnyWindow<TPresenter>();
        if (presenter != null && _presenterToWindowMap.TryGetValue(presenter, out var window)) window.Close();
    }

    public void Dispose()
    {
        _logger.LogInformation("UIManager Disposing ...");
        while (_windowStack.TryPop(out var window)) window.Close(); // Dispose windows
        foreach (var current in _presenterToTabItemMap)
        {
            _tabControl.Items.Remove(current.Value);
            current.Key.Dispose();
        }

        _presenterToTabItemMap.Clear();
        _presenterToWindowMap.Clear();
        var backgroundDispatcher =
            (BackgroundThreadDispatcher)_serviceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
        backgroundDispatcher.Stop();
    }

    public void SetTabControl(TabControl tabControl)
    {
        _tabControl = tabControl;
    }

    private IPresenter GetPresenterFromType(Type presenterType)
    {
        return _serviceProvider.GetRequiredService(presenterType) as IPresenter;
    }

    private Window CreateDialog(IPresenter presenter, Dictionary<string, object> parameters)
    {
        var view = presenter.GetView();
        var userControl = view as UserControl;
        var window = new DefaultDialog();
        window.SetContent(userControl);
        window.Title = view.Title;
        SetWindowParameters(parameters, window);
        return window;
    }

    private Window CreateWindow(IPresenter presenter, Dictionary<string, object> parameters)
    {
        var view = presenter.GetView();
        var userControl = view as UserControl;
        var window = new DefaultWindow();
        window.SetContent(userControl);
        window.Title = view.Title;
        SetWindowParameters(parameters, window);

        return window;
    }

    private void SetWindowParameters(Dictionary<string, object> parameters, Window window)
    {
        if (parameters != null)
        {
            if (parameters.TryGetValue("Width", out var width))
                if (width is int windowWidth)
                    window.Width = windowWidth;

            if (parameters.TryGetValue("Height", out var height))
                if (height is int windowHeight)
                    window.Height = windowHeight;

            if (parameters.TryGetValue("WindowStartupLocation", out var startupLocation))
                if (startupLocation is WindowStartupLocation location)
                    window.WindowStartupLocation = location;

            if (parameters.TryGetValue("WindowState", out var windowState))
                if (windowState is WindowState state)
                    window.WindowState = state;

            if (parameters.TryGetValue("SystemDecoration", out var decoration))
                if (decoration is SystemDecorations windowDecoration)
                    window.SystemDecorations = windowDecoration;
        }
    }

    private void AddWindowToCollections(IPresenter presenter, Window window)
    {
        _presenterToWindowMap[presenter] = window;
        _windowStack.Push(window);

        window.Closed += (_, _) =>
        {
            _windowStack.Pop();
            _presenterToWindowMap.Remove(presenter);
            presenter.Dispose();
        };
    }

    private void HandleExistingWindow(Window window)
    {
        if (window.IsVisible)
            window.Activate();
        else
            window.Show();
    }

    private void HandleExistingTabItem(DefaultTabItem tabItem)
    {
        _tabControl.SelectedItem = tabItem;
    }

    private TPresenter GetPresenterFromAnyWindow<TPresenter>() where TPresenter : IPresenter
    {
        foreach (var presenter in _presenterToWindowMap.Keys)
            if (presenter is TPresenter presenterToWindow)
                return presenterToWindow;

        return default;
    }

    private bool HasExistingPresenterInWindow(Type presenterType)
    {
        foreach (var presenter in _presenterToWindowMap.Keys)
            if (presenter.GetType() == presenterType)
                return true;

        return false;
    }

    private DefaultTabItem HasExistingPresenterInTabItems(Type presenterType)
    {
        foreach (var map in _presenterToTabItemMap)
        {
            var interfaces = map.Key.GetType().GetInterfaces();
            if (interfaces.Any(i => i == presenterType)) return map.Value;
        }

        return null;
    }

    public void SetNativeApplication(IClassicDesktopStyleApplicationLifetime applicationLifetime,
        bool useAuthentication)
    {
        _useAuthentication = useAuthentication;
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        _applicationLifetime.ShutdownRequested += ApplicationLifetimeOnShutdownRequested;
        _applicationLifetime.Exit += ApplicationLifetimeOnExit;
        _applicationLifetime.Startup += ApplicationLifetimeOnStartup;
    }


    private void ApplicationLifetimeOnStartup(object sender, ControlledApplicationLifetimeStartupEventArgs e)
    {
        _logger.LogInformation("UIManager Startup");
        if (_useAuthentication)
            ShowWindow<IAuthenticationPresenter>(new Dictionary<string, object>()
            {
                { "Width", 500 },
                { "Height", 300 },
                { "SystemDecoration", SystemDecorations.None },
                { "WindowStartupLocation", WindowStartupLocation.CenterScreen }
            });
        else
            ShowWindow<IMainPresenter>(new Dictionary<string, object>
            {
                { "WindowState", WindowState.Maximized },
                { "WindowStartupLocation", WindowStartupLocation.CenterScreen }
            });
    }

    private void ApplicationLifetimeOnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _logger.LogInformation("UIManager Exit");
        Dispose();
    }

    private void ApplicationLifetimeOnShutdownRequested(object sender, ShutdownRequestedEventArgs e)
    {
        _logger.LogInformation("UIManager ShutdownRequested");
    }
}