using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Abstraction.Models;

public interface IViewModel : IDataDirtyState
{
    Guid Id { get; }
}