namespace BitCrafts.Infrastructure.Abstraction.Threading;

/// <summary>
///     Defines an interface for dispatching actions and functions to a specific thread.
///     This is useful for ensuring that certain operations are performed on a particular thread,
///     such as the UI thread.
/// </summary>
public interface IThreadDispatcher
{
    /// <summary>
    ///     Invokes an action on the dispatcher's thread. Execution is synchronous.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    void Invoke(Action action);

    /// <summary>
    ///     Invokes an action on the dispatcher's thread. Execution is asynchronous.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task InvokeAsync(Action action);

    /// <summary>
    ///     Invokes a function on the dispatcher's thread and returns the result. Execution is asynchronous.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <returns>A Task that represents the asynchronous operation and yields the result.</returns>
    Task<T> Invoke<T>(Func<T> func);

    /// <summary>
    ///     Invokes an asynchronous function on the dispatcher's thread and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="taskFunc">The asynchronous function to execute.</param>
    /// <returns>A Task that represents the asynchronous operation and yields the result.</returns>
    Task<T> InvokeTaskAsync<T>(Func<Task<T>> taskFunc);

    /// <summary>
    ///     Invokes an asynchronous function on the dispatcher's thread.
    /// </summary>
    /// <param name="taskFunc">The asynchronous function to execute.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task InvokeTaskAsync(Func<Task> taskFunc);
}