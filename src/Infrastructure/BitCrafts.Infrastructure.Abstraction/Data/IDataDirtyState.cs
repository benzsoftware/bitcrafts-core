namespace BitCrafts.Infrastructure.Abstraction.Data;

public interface IDataDirtyState
{
    bool IsDirty { get; }
    void MarkAsDirty();
    void ResetDirtyState();
}