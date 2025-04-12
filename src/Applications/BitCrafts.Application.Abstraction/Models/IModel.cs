using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Abstraction.Models;

public interface IModel : IDataDirtyState
{
    Guid Id { get; }
}