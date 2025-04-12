using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Avalonia.Controls.Loading;
using BitCrafts.Application.Avalonia.Controls.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Models;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;

namespace BitCrafts.Module.Demo.DemoModule.Views;

public partial class DisplayUserAccountsView : BaseView, IDisplayUserAccountsView
{
    private readonly ObservableCollection<User> _users = new();

    protected override IModel UpdateModelFromInputsCore()
    {
        return Model;
    }

    protected override LoadingControl LoadingIndicator => null;
 

    public DisplayUserAccountsView()
    {
        InitializeComponent();
    }

    /*protected override void OnDataDisplayed()
    {
        _users.Clear();
        foreach (var user in Model.Users)
            _users.Add(user);
    }*/

    /* protected override void OnAppeared()
     {
         UsersDataGrid.ItemsSource = _users;
     }

     protected override void OnDisappeared()
     {
         _users.Clear();
     }*/

    /*
    private void CreateUserButton_OnClick(object sender, RoutedEventArgs e)
    {
        CreateUser?.Invoke(this, EventArgs.Empty);
    }

    private void UsersDataGrid_OnRowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
    {
        var user = e.Row.DataContext as User;
        if (user != null) UpdateUser?.Invoke(this, user);
    }*/
/*
    private void UsersDataGrid_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete) DeleteSelectedUsers();
    }
*/
/*
    private void DeleteSelectedUsers()
    {
        var users = UsersDataGrid.SelectedItems.Cast<User>().ToList();
        if (users.Any())
        {
            DeleteUser?.Invoke(this, users);
            foreach (var user in users) _users.Remove(user);
        }
    }*/
/*
    private void DeleteUserButton_OnClick(object sender, RoutedEventArgs e)
    {
        DeleteSelectedUsers();
    }
*/
    /* private void UsersDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
     {
         if (UsersDataGrid.SelectedItems.Count > 0)
             DeleteUserButton.IsVisible = true;
         else
             DeleteUserButton.IsVisible = false;

         e.Handled = true;
     }*/
/*
    private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
    {
        Refresh?.Invoke(this, EventArgs.Empty);
    }*/
    private void DeleteUserButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void CreateUserButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}