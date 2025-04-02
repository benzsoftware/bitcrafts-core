using System.Collections.Concurrent;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using BitCrafts.Application.Abstraction;
using BitCrafts.Application.Abstraction.Application.Managers;
using BitCrafts.Application.Abstraction.Application.Presenters;
using BitCrafts.Application.Avalonia.Dialogs;
using BitCrafts.Application.Avalonia.Presenters;
using BitCrafts.Application.Avalonia.Views;
using BitCrafts.Infrastructure.Abstraction.Threading;
using BitCrafts.Infrastructure.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Application.Avalonia.Managers;

public sealed class AvaloniaUiManager : IUiManager
{
    private readonly ILogger<AvaloniaUiManager> _logger;
    private readonly ConcurrentDictionary<IPresenter, TabItem> _presenterToTabItemMap = new();
    private readonly ConcurrentDictionary<IPresenter, Window> _presenterToWindowMap = new();
    private readonly IServiceProvider _serviceProvider;
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

    public async Task ShowErrorMessageAsync(string title, string message)
    {
        if (!Dispatcher.UIThread.CheckAccess())
        {
            await Dispatcher.UIThread.InvokeAsync(async void () =>
                await ShowErrorMessageAsync(title, message));
            return;
        }

        var dialog = new ErrorMessageDialog();
        dialog.SetMessage(title, message);
        await dialog.ShowDialog(_activeWindow);
    }

    public async Task ShowErrorMessageAsync(string title, Exception exception)
    {
        if (!Dispatcher.UIThread.CheckAccess())
        {
            await Dispatcher.UIThread.InvokeAsync(async void () =>
                await ShowErrorMessageAsync(title, exception));
            return;
        }

        var dialog = new ErrorMessageDialog();
        dialog.SetMessage(title, exception.Message);
        await dialog.ShowDialog(_activeWindow);
    }

    public async Task ShowInTabControlAsync<TPresenter>(Dictionary<string, object> parameters)
        where TPresenter : class, IPresenter
    {
        var presenterType = typeof(TPresenter);
        await ShowInTabControlAsync(presenterType, parameters);
    }

    public async Task ShowInTabControlAsync(Type presenterType, Dictionary<string, object> parameters)
    {
        if (!Dispatcher.UIThread.CheckAccess())
        {
            await Dispatcher.UIThread.InvokeAsync(async void () =>
                await ShowInTabControlAsync(presenterType, parameters));
            return;
        }

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
        var tabItem = CreateTabItem(presenter);
        _presenterToTabItemMap[presenter] = tabItem;
        _tabControl.Items.Add(tabItem);
    }

    private TabItem CreateTabItem(IPresenter presenter)
    {
        var userControl = presenter.GetView() as UserControl;
        var view = presenter.GetView();
        var tabItem = new TabItem();
        var headerGrid = new Grid
        {
            MinWidth = 150,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(2)
        };
 
        headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
 
        var titleTextBox = new TextBlock
        {
            Name = "TitleTextBox",
            Text = view.Title,
            FontSize = 12,
            FontWeight = FontWeight.Bold,
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(titleTextBox, 0);
        headerGrid.Children.Add(titleTextBox);

        var closeButton = new Button
        {
            Content = "x",
            BorderThickness = new Thickness(0),
            BorderBrush = new SolidColorBrush(Colors.DarkSlateGray),
            CornerRadius = new CornerRadius(0),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        closeButton.Click += async void (_, _) =>
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _tabControl.Items.Remove(tabItem);
                _presenterToTabItemMap.Remove(presenter, out _);
                presenter.Dispose();
            });
        };
        Grid.SetColumn(closeButton, 1);
        headerGrid.Children.Add(closeButton);

        tabItem.Header = headerGrid;

        tabItem.Content = userControl;
        return tabItem;
    }

    public Task ShutdownAsync()
    {
        _logger.LogInformation("Shutting down application...");
        if (_activeWindow != null) _activeWindow.Close();

        if (_rootWindow != null) _rootWindow.Close();

        return Task.CompletedTask;
    }

    public async Task ShowWindowAsync(Type presenterType, Dictionary<string, object> parameters = null)
    {
        if (!Dispatcher.UIThread.CheckAccess())
        {
            await Dispatcher.UIThread
                .InvokeAsync(async void () =>
                    await ShowWindowAsync(presenterType, parameters));
            return;
        }

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

    public async Task ShowWindowAsync<TPresenter>(Dictionary<string, object> parameters = null)
        where TPresenter : class, IPresenter
    {
        var presenterType = typeof(TPresenter);
        await ShowWindowAsync(presenterType, parameters);
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

        await dialog.ShowDialog(_activeWindow);
    }

    public async Task CloseDialogAsync<TPresenter>() where TPresenter : class, IPresenter
    {
        var presenterType = typeof(TPresenter);
        await CloseDialogAsync(presenterType);
    }

    public async Task CloseDialogAsync(Type presenterType)
    {
        var presenter = GetPresenterFromAnyWindow(presenterType);
        if (presenter == null)
        {
            await ShowErrorMessageAsync("Error",
                $"Could not create the required window component ({presenterType.Name}).");
            return;
        }

        if (_presenterToWindowMap.TryGetValue(presenter, out var window)) window.Close();
    }

    public void Dispose()
    {
        _logger.LogInformation("UIManager Disposing ...");
        foreach (var current in _presenterToTabItemMap)
        {
            _tabControl.Items.Remove(current.Value);
            current.Key.Dispose();
        }

        var windowsToClose = _presenterToWindowMap.Values.ToList();
        foreach (var window in windowsToClose) window?.Close();

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
        var presenter = _serviceProvider.GetService(presenterType) as IPresenter;
        if (presenter == null)
            _logger.LogError("Presenter type {PresenterType} not registered or could not be resolved.",
                presenterType.FullName);

        return presenter;
    }


    private Window CreateDialog(IPresenter presenter, Dictionary<string, object> parameters = null)
    {
        var view = presenter.GetView();
        var userControl = view as UserControl;
        var window = new DefaultDialog();
        window.SetContent(userControl);
        window.Title = view.Title;
        SetWindowParameters(parameters, window);
        return window;
    }

    private Window CreateWindow(IPresenter presenter, Dictionary<string, object> parameters = null)
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
            if (parameters.TryGetValue(Constants.WindowWidthParameterName, out var width))
                if (width is int windowWidth)
                    window.Width = windowWidth;

            if (parameters.TryGetValue(Constants.WindowHeightParameterName, out var height))
                if (height is int windowHeight)
                    window.Height = windowHeight;

            if (parameters.TryGetValue(Constants.WindowStartupLocationParameterName, out var startupLocation))
                if (startupLocation is WindowStartupLocation location)
                    window.WindowStartupLocation = location;

            if (parameters.TryGetValue(Constants.WindowStateParameterName, out var windowState))
                if (windowState is WindowState state)
                    window.WindowState = state;

            if (parameters.TryGetValue(Constants.WindowSystemDecorationParameterName, out var decoration))
                if (decoration is SystemDecorations windowDecoration)
                    window.SystemDecorations = windowDecoration;
        }
    }

    private void AddWindowToCollections(IPresenter presenter, Window window)
    {
        _presenterToWindowMap[presenter] = window;

        window.Closed += (_, _) =>
        {
            if (_activeWindow == window) _activeWindow = _rootWindow;

            _presenterToWindowMap.Remove(presenter, out _);
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

    private void HandleExistingTabItem(TabItem tabItem)
    {
        _tabControl.SelectedItem = tabItem;
    }

    private TPresenter GetPresenterFromAnyWindow<TPresenter>() where TPresenter : IPresenter
    {
        var presenterType = typeof(TPresenter);
        return (TPresenter)GetPresenterFromAnyWindow(presenterType);
    }

    private IPresenter GetPresenterFromAnyWindow(Type presenterType)
    {
        return _presenterToWindowMap.Keys
            .FirstOrDefault(p => presenterType.IsAssignableFrom(p.GetType()));
    }

    private bool HasExistingPresenterInWindow(Type presenterType)
    {
        return _presenterToWindowMap.Keys.Any(p => presenterType.IsAssignableFrom(p.GetType()));
    }

    private TabItem HasExistingPresenterInTabItems(Type presenterType)
    {
        return _presenterToTabItemMap
            .FirstOrDefault(map => presenterType.IsAssignableFrom(map.Key.GetType()))
            .Value;
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


    private async void ApplicationLifetimeOnStartup(object sender, ControlledApplicationLifetimeStartupEventArgs e)
    {
        _logger.LogInformation("UIManager Startup");
        if (_useAuthentication)
            await ShowWindowAsync<IAuthenticationPresenter>(new Dictionary<string, object>()
            {
                { Constants.WindowWidthParameterName, 500 },
                { Constants.WindowHeightParameterName, 300 },
                { Constants.WindowSystemDecorationParameterName, SystemDecorations.None },
                { Constants.WindowStateParameterName, WindowState.Normal }
            });
        else
            await ShowWindowAsync<IMainPresenter>();
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