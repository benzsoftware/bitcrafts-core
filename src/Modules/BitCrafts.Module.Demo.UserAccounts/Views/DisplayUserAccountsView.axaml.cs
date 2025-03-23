using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using BitCrafts.Infrastructure.Avalonia.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;

namespace BitCrafts.Module.Demo.UserAccounts.Views;

public partial class DisplayUserAccountsView : BaseControl, IDisplayUserAccountsView
{
    private readonly ObservableCollection<User> _users = new();

    public DisplayUserAccountsView()
    {
        InitializeComponent();
        Title = "Display User Accounts";
    }

    public void RefreshUsers(IEnumerable<User> users)
    {
        _users.Clear();
        foreach (var user in users) _users.Add(user);
    }

    public void AppendUser(User user)
    {
        _users.Add(user);
    }

    public event EventHandler CreateUser;
    public event EventHandler<User> UpdateUser;
    public event EventHandler<IEnumerable<User>> DeleteUser;

    protected override void OnAppeared()
    {
        UsersDataGrid.ItemsSource = _users;
    }

    protected override void OnDisappeared()
    {
        _users.Clear();
    }


    private void CreateUserButton_OnClick(object sender, RoutedEventArgs e)
    {
        CreateUser?.Invoke(this, EventArgs.Empty);
    }

    private void UsersDataGrid_OnRowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
    {
        var user = e.Row.DataContext as User;
        if (user != null) UpdateUser?.Invoke(this, user);
    }

    private void UsersDataGrid_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete)
        {
        }
    }

    private void DeleteSelectedUsers()
    {
        var users = UsersDataGrid.SelectedItems.Cast<User>().ToList();
        if (users.Any())
        {
            DeleteUser?.Invoke(this, users);
            foreach (var user in users) _users.Remove(user);
        }
    }

    private void UpdateUserButton_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void DeleteUserButton_OnClick(object sender, RoutedEventArgs e)
    {
        DeleteSelectedUsers();
    }

    private void UsersDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UsersDataGrid.SelectedItems.Count > 0)
        {
            DeleteUserButton.IsVisible = true;
            UpdateUserButton.IsVisible = true;
        }
        else if (UsersDataGrid.SelectedItems.Count > 1)
        {
            DeleteUserButton.IsVisible = true;
            UpdateUserButton.IsVisible = false;
        }
        else
        {
            DeleteUserButton.IsVisible = false;
            UpdateUserButton.IsVisible = false;
        }

        e.Handled = true;
    }

    private void UsersDataGrid_OnLostFocus(object sender, RoutedEventArgs e)
    {
        UsersDataGrid.SelectedIndex = -1;
    }
}