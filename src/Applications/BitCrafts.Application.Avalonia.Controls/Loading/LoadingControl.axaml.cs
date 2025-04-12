using Avalonia.Controls;

namespace BitCrafts.Application.Avalonia.Controls.Loading;

public partial class LoadingControl : UserControl
{
    public LoadingControl()
    {
        InitializeComponent();
    }

    public void SetLoading(bool isLoading, string message = "")
    {
        LoadingOverlay.IsVisible = isLoading;
        LoadingMessageTextBox.Text = message;
    }
}