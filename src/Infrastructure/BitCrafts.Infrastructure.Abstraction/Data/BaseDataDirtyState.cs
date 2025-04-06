namespace BitCrafts.Infrastructure.Abstraction.Data;

public abstract class BaseDataDirtyState : IDataDirtyState
{
    private bool _isDirty;

    public bool IsDirty => _isDirty;

    public void MarkAsDirty()
    {
        _isDirty = true;
    }

    public void ResetDirtyState()
    {
        _isDirty = false;
    }
}