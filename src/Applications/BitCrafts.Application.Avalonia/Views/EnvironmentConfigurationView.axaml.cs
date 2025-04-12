using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Events;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Application.Avalonia.Controls.Loading;
using BitCrafts.Application.Avalonia.Controls.Views;
using BitCrafts.Infrastructure.Abstraction.Extensions;
using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Application.Avalonia.Views;

public partial class EnvironmentConfigurationView : BaseView, IEnvironmentConfigurationView
{
    private EnvironmentConfigurationModel ViewModel => Model as EnvironmentConfigurationModel;

    public EnvironmentConfigurationView()
    {
        InitializeComponent();
    }


    protected void OnDataDisplayed()
    {
        EnvironmentsComboBox.ItemsSource = ViewModel.Environments;
        DatabaseTypeComboBox.ItemsSource = Enum.GetNames(typeof(DatabaseProviderType));
        EnvironmentTypeComboBox.ItemsSource = Enum.GetNames(typeof(EnvironmentType));
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        EventAggregator.Publish(ViewEvents.Base.CloseWindowEventName);
    }

    private EnvironmentConfiguration GetConfigurationFromUserInput()
    {
        return new EnvironmentConfiguration()
        {
            ConnectionString = ConnectionStringTextBox.Text.TrimOrEmpty(),
            Description = DescriptionTextBox.Text.TrimOrEmpty(),
            Name = NameTextBox.Text.TrimOrEmpty(),
            Type = Enum.Parse<EnvironmentType>(EnvironmentTypeComboBox.SelectedItem.ToString()),
            DatabaseProvider = Enum.Parse<DatabaseProviderType>(DatabaseTypeComboBox.SelectedItem.ToString())
        };
    }

    private void DisplayConfiguration(EnvironmentConfiguration environmentConfiguration)
    {
        if (environmentConfiguration == null)
            return;

        ConnectionStringTextBox.Text = environmentConfiguration.ConnectionString;
        DescriptionTextBox.Text = environmentConfiguration.Description;
        NameTextBox.Text = environmentConfiguration.Name;

        if (EnvironmentTypeComboBox.Items.Contains(environmentConfiguration.Type.ToString()))
            EnvironmentTypeComboBox.SelectedItem = environmentConfiguration.Type.ToString();

        if (DatabaseTypeComboBox.Items.Contains(environmentConfiguration.DatabaseProvider.ToString()))
            DatabaseTypeComboBox.SelectedItem = environmentConfiguration.DatabaseProvider.ToString();
    }


    private void NewEnvironmentButton_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.SetEditingEnvironment(GetConfigurationFromUserInput());
        ViewModel.Environments.Add(ViewModel.EditingEnvironment);
    }

    private void EnvironmentsComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.AddedItems.OfType<EnvironmentConfiguration>().SingleOrDefault();
        if (selectedItem != null)
        {
            ViewModel.SetSelectedEnvironment(selectedItem);
            ViewModel.SetEditingEnvironment(selectedItem);
            DisplayConfiguration(ViewModel.EditingEnvironment);
        }
    }

    protected override IModel UpdateModelFromInputsCore()
    {
        return ViewModel;
    }

    protected override LoadingControl LoadingIndicator => BusyControl;
}