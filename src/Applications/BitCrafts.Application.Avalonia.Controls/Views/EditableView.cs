using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Avalonia.Controls.Views;

public abstract class EditableView<TModel> : LoadableView<TModel>, IEditableView<TModel>
    where TModel : class, IViewModel, new()
{
    public event EventHandler<TModel> SaveRequested;
    public event EventHandler CancelRequested;

    protected virtual void RequestSave(TModel modelToSave)
    {
        SaveRequested?.Invoke(this, modelToSave);
    }

    protected virtual void RequestCancel()
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }
}