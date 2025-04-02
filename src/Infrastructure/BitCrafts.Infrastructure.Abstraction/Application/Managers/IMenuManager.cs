namespace BitCrafts.Infrastructure.Abstraction.Application.Managers;

public interface IMenuManager
{
    void AddMenuItem(string title, Action action = null);
    void AddMenuItemInSubItem(string parentItem, string title, Action action = null);

    void AddSeparatorInSubItem(string parentItem);
}