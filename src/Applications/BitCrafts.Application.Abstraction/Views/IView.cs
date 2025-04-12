using System.ComponentModel.DataAnnotations;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Abstraction.Views;

public interface IView : IEventAware, IDisposable
{
    bool IsVisible { get; }
    bool IsBusy { get; }
    string Title { get; set; }
    IModel Model { get; }
    IDataValidator DataValidator { get; }
    bool SetModel(IModel model);
    void UpdateModelFromInputs();
    void Clear();
    void SetVisible(bool visible);
    void SetBusy(bool busy, string message = "");
    bool ValidateModel(out List<ValidationResult> validationResults);
}