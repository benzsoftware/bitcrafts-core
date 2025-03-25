using Avalonia.Controls;

namespace BitCrafts.Infrastructure.Application.Dialogs;

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