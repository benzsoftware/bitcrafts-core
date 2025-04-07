using Avalonia.Controls;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Avalonia.Controls.Views;

public abstract class LoadableView<TModel> : BaseView, ILoadableView<TModel>
    where TModel : class, IViewModel, new()
{
    protected abstract Control LoadingIndicator { get; }
    protected abstract TextBlock ErrorTextBlock { get; }
    protected TModel Model { get; set; }

    public virtual void ShowLoading(string message = "Loading in progress...")
    {
        if (LoadingIndicator != null)
        {
            LoadingIndicator.IsVisible = true;

            if (LoadingIndicator is ContentControl contentControl) contentControl.Content = message;
        }
    }

    protected virtual void SetModel()
    {
        Model = new TModel();
    }

    public virtual void HideLoading()
    {
        if (LoadingIndicator != null) LoadingIndicator.IsVisible = false;
    }

    public virtual void DisplayData(TModel model)
    {
        Model = model;
        OnDataDisplayed(model);
    }

    public void ShowError(string message = "Error occured")
    {
        if (ErrorTextBlock != null)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.IsVisible = true;
        }
    }

    public virtual void HideError()
    {
        if (ErrorTextBlock != null)
            ErrorTextBlock.IsVisible = false;
    }

    protected override void OnAppeared()
    {
        HideError();
    }


    protected abstract void OnDataDisplayed(TModel model);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            Model = default;

        base.Dispose(disposing);
    }
}