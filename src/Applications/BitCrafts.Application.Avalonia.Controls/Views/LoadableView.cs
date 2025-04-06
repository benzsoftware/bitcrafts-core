using Avalonia.Controls;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Avalonia.Controls.Views;

public abstract class LoadableView<TModel> : BaseView, ILoadableView<TModel>
    where TModel : class, IViewModel, new()
{
    protected abstract Control LoadingIndicator { get; }
    protected TModel Model { get; set; }
    protected abstract TextBlock ErrorTextBlock { get; }

    public virtual void ShowLoading(string message = "Loading in progress...")
    {
        if (LoadingIndicator != null)
        {
            LoadingIndicator.IsVisible = true;

            if (LoadingIndicator is ContentControl contentControl) contentControl.Content = message;
        }
    }

    public virtual void HideLoading()
    {
        if (LoadingIndicator != null) LoadingIndicator.IsVisible = false;
    }

    public virtual void DisplayData(TModel model)
    {
        Model = model;

        // La logique spécifique d'affichage doit être implémentée dans les classes dérivées
        OnDataDisplayed(model);
    }

    /// <summary>
    /// Méthode appelée après la mise à jour du modèle dans la vue
    /// </summary>
    protected abstract void OnDataDisplayed(TModel model);

    public override void ShowError(string message = "Error occured")
    {
        if (ErrorTextBlock != null)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.IsVisible = true;
        }
    }

    public virtual void HideError()
    {
        if (ErrorTextBlock != null) ErrorTextBlock.IsVisible = false;
    }

    protected override void OnAppeared()
    {
        HideError();
    }

    protected override void OnDisappeared()
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) Model = default;

        base.Dispose(disposing);
    }
}