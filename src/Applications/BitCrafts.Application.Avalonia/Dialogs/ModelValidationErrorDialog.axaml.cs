using System.ComponentModel.DataAnnotations;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace BitCrafts.Application.Avalonia.Dialogs;

public partial class ModelValidationErrorDialog : Window
{
    public ModelValidationErrorDialog()
    {
        InitializeComponent();
    }

    public void SetValidationErrors(List<ValidationResult> validationResults)
    {
        if (validationResults != null && validationResults.Count > 0)
        {
            ValidationErrorsList.ItemsSource = validationResults;
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}