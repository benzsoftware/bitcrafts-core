using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BitCrafts.Infrastructure.Application.Views;

public partial class DefaultTabItem : TabItem
{
    public DefaultTabItem()
    {
        InitializeComponent();
    }

    public event EventHandler Close;

    public void SetContent(UserControl userControl)
    {
        MainContent.Content = userControl;
    }

    public void SetTitle(string title)
    {
        TitleTextBox.Text = title;
    }

    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        Close?.Invoke(this, EventArgs.Empty);
    }
}