using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Abstraction.Models;

public abstract class BaseViewModel : BaseDataDirtyState, IViewModel
{
    public Guid Id { get; } = Guid.NewGuid();
}