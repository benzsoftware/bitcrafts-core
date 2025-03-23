namespace BitCrafts.Infrastructure.Abstraction.Threading;

/// <summary>
///     Defines an interface for a thread dispatcher that manages a background thread.
///     This is useful for performing long-running or non-UI-related operations
///     on a separate thread to keep the UI responsive.
/// </summary>
public interface IBackgroundThreadDispatcher : IThreadDispatcher
{
}