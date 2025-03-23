namespace BitCrafts.Infrastructure.Abstraction.UseCases;

/// <summary>
///     Defines an interface for use cases that encapsulate application logic.
///     Use cases define the operations that a user can perform within the application.
///     This interface is for use cases that take an input and produce an output.
/// </summary>
/// <typeparam name="TInput">The type of the input for the use case.</typeparam>
/// <typeparam name="TOutput">The type of the output from the use case.</typeparam>
public interface IUseCase<TInput, TOutput> : IDisposable
{
    /// <summary>
    ///     Executes the use case asynchronously.
    /// </summary>
    /// <param name="input">The input for the use case.</param>
    /// <returns>A Task that represents the asynchronous operation and yields the output.</returns>
    Task<TOutput> ExecuteAsync(TInput input);
}

/// <summary>
///     Defines an interface for use cases that encapsulate application logic.
///     Use cases define the operations that a user can perform within the application.
///     This interface is for use cases that take an input and do not produce an output.
/// </summary>
/// <typeparam name="TInput">The type of the input for the use case.</typeparam>
public interface IUseCase<TInput> : IDisposable
{
    /// <summary>
    ///     Executes the use case asynchronously.
    /// </summary>
    /// <param name="input">The input for the use case.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task ExecuteAsync(TInput input);
}

/// <summary>
///     Defines an interface for use cases that encapsulate application logic.
///     Use cases define the operations that a user can perform within the application.
///     This interface is for use cases that do not take any input and do not produce an output.
/// </summary>
public interface IUseCase : IDisposable
{
    /// <summary>
    ///     Executes the use case asynchronously.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task ExecuteAsync();
}

/// <summary>
///     Defines an interface for use cases that encapsulate application logic.
///     Use cases define the operations that a user can perform within the application.
///     This interface is for use cases that do not take any input but produce a result.
/// </summary>
/// <typeparam name="TResult">The type of the result from the use case.</typeparam>
public interface IUseCaseWithResult<TResult> : IDisposable
{
    /// <summary>
    ///     Executes the use case asynchronously and returns a result.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation and yields the result.</returns>
    Task<TResult> ExecuteAsync();
}