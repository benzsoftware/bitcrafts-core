using Avalonia.Controls;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Avalonia.Controls.Views;

public abstract class EditableView<TModel> : LoadableView<TModel>, IEditableView<TModel>
    where TModel : class, IViewModel, new()
{
    public event EventHandler<TModel> SaveRequested;
    public event EventHandler CancelRequested;

    protected abstract Control SuccessIndicator { get; }

    public virtual void ShowSuccess(string message)
    {
        if (SuccessIndicator != null)
        {
            SuccessIndicator.IsVisible = true;

            if (SuccessIndicator is ContentControl contentControl) contentControl.Content = message;
        }
    }

    public virtual void HideSuccess()
    {
        if (SuccessIndicator != null) SuccessIndicator.IsVisible = false;
    }

    protected virtual void RequestSave(TModel modelToSave)
    {
        SaveRequested?.Invoke(this, modelToSave);
    }

    protected virtual void RequestCancel()
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnAppeared()
    {
        base.OnAppeared();
        HideSuccess();
    }
}