using Avalonia.Controls;

namespace BitCrafts.Application.Avalonia.Views;

public partial class DefaultDialog : Window
{
    public DefaultDialog()
    {
        InitializeComponent();
    }

    public void SetContent(UserControl control)
    {
        DefaultContentPresenter.Content = control;
    }
}