using BitCrafts.Application.Abstraction.Models;

namespace BitCrafts.Application.Abstraction.Views;

public interface ILoadableView<TModel> : IView where TModel : class, IViewModel, new()
{
    void ShowLoading(string message = "Loading...");
    void HideLoading();
    void ShowError(string message = "Error occured");
    void HideError();
    void DisplayData(TModel model);
}