using Avalonia.Controls;

namespace BitCrafts.Application.Avalonia.Dialogs;

public partial class ErrorMessageDialog : Window
{
    public ErrorMessageDialog()
    {
        InitializeComponent();
    }

    public void SetMessage(string title, string message)
    {
        Title = title;
        ErrorMessageTextBlock.Text = message;
    }
}