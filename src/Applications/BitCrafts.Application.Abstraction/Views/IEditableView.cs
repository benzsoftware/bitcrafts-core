using BitCrafts.Application.Abstraction.Models;

namespace BitCrafts.Application.Abstraction.Views;

public interface IEditableView<TModel> : ILoadableView<TModel> where TModel : class, IViewModel, new()
{
    event EventHandler<TModel> SaveRequested;

    event EventHandler CancelRequested;
}