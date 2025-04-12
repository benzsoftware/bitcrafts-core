using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Abstraction.Models;

public abstract class BaseModel : BaseDataDirtyState, IModel
{
    public Guid Id { get; } = Guid.NewGuid();
    
}