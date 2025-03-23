using Avalonia.Controls;

namespace BitCrafts.Infrastructure.Application.Views;

public partial class DefaultWindow : Window
{
    public DefaultWindow()
    {
        InitializeComponent();
    }

    public void SetContent(UserControl control)
    {
        DefaultContentPresenter.Content = control;
    }
}