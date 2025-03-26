using Material.Icons;

namespace BitCrafts.Infrastructure.Abstraction.Application.Managers;

public interface IMenuManager
{
    void AddMenuItem(string title, MaterialIconKind iconKind, Action action = null);
    void AddMenuItemInSubItem(string parentItem, string title, MaterialIconKind iconKind, Action action = null);

    void AddSeparatorInSubItem(string parentItem);
}