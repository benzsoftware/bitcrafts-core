using Avalonia.Controls;
using Avalonia.Threading;
using BitCrafts.Infrastructure.Abstraction.Application.Managers;
using Material.Icons;
using Material.Icons.Avalonia;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Application.Managers;

public sealed class AvaloniaMenuManager : IMenuManager
{
    private readonly ILogger<AvaloniaMenuManager> _logger;
    private Menu _menu;
    private const double DefaultIconWidth = 16;
    private const double DefaultIconHeight = 16;

    public AvaloniaMenuManager(ILogger<AvaloniaMenuManager> logger)
    {
        _logger = logger;
    }

    public void SetMenuControl(Menu menu)
    {
        _menu = menu ?? throw new ArgumentNullException(nameof(menu));
    }

    public void AddMenuItem(string title, Action action = null)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            if (_menu == null)
            {
                _logger.LogError("Menu control has not been set.");
                return;
            }

            var menuItem = CreateMenuItem(title, action);
            _menu.Items.Add(menuItem);
        });
    }

    public void AddMenuItemInSubItem(string parentItem, string title, Action action = null)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            if (_menu == null)
            {
                _logger.LogError("Menu control has not been set.");
                return;
            }

            var parentMenuItem = FindMenuItemRecursively(_menu.Items, parentItem);

            if (parentMenuItem == null)
            {
                _logger.LogWarning($"Parent menu item '{parentItem}' not found for adding child '{title}'.");
                return;
            }

            var newMenuItem = CreateMenuItem(title, action);
            parentMenuItem.Items.Add(newMenuItem);
        });
    }

    private void AddSeparator()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            if (_menu == null) return;

            _menu.Items.Add(new Separator());
        });
    }

    public void AddSeparatorInSubItem(string parentItem)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            if (_menu == null) return;

            var parentMenuItem = FindMenuItemRecursively(_menu.Items, parentItem);
            if (parentMenuItem == null) return;

            parentMenuItem.Items.Add(new Separator());
        });
    }

    private MenuItem CreateMenuItem(string title, Action action = null)
    {
        var menuItem = new MenuItem()
        {
            Header = title
        };

        if (action != null) menuItem.Click += (sender, args) => action();

        return menuItem;
    }

    private MenuItem FindMenuItemRecursively(IEnumerable<object> items, string headerToFind)
    {
        if (items == null) return null;

        var menuItems = items.OfType<MenuItem>();

        foreach (var menuItem in menuItems)
        {
            var title = menuItem.Header?.ToString();
            if (title?.Contains(headerToFind) == true) return menuItem;

            var foundInChildren = FindMenuItemRecursively(menuItem.Items, headerToFind);
            if (foundInChildren != null) return foundInChildren;
        }

        return null;
    }
}