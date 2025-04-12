using Avalonia.Controls;
using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Avalonia.Controls.Views;

public abstract class LoadableView : BaseView, ILoadableView
{
    protected abstract Control LoadingIndicator { get; }
    protected abstract TextBlock MessageTextBlock { get; }

    public override void SetBusy(bool busy)
    {
        base.SetBusy(busy);
        if (LoadingIndicator != null)
        {
            LoadingIndicator.IsVisible = IsBusy;
        }
    }

    public void ShowLoadingMessage(string message = "Loading...")
    {
        if (MessageTextBlock is { } textBlock) textBlock.Text = message;
    }
}