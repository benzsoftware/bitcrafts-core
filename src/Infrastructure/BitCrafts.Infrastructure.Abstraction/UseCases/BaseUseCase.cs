using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace BitCrafts.Infrastructure.Abstraction.UseCases;

/// <summary>
///     Provides an abstract base class for use cases that take an input and produce an output.
///     This class implements the IUseCase&lt;TInput, TOutput&gt; interface and provides
///     a default ExecuteAsync implementation.
/// </summary>
/// <typeparam name="TInput">The type of the input for the use case.</typeparam>
/// <typeparam name="TOutput">The type of the output from the use case.</typeparam>
public abstract class BaseUseCase<TInput, TOutput> : IUseCase<TInput, TOutput>
{
    protected IBackgroundThreadDispatcher BackgroundThreadDispatcher { get; }

    protected BaseUseCase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        BackgroundThreadDispatcher = ServiceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
    }

    protected IServiceProvider ServiceProvider { get; }

    /// <inheritdoc />
    public async Task<TOutput> ExecuteAsync(TInput input)
    {
        return await BackgroundThreadDispatcher.InvokeTaskAsync(async () =>
        {
            return await ExecuteCoreAsync(input).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    /// <summary>
    ///     Disposes of any resources used by the use case.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Provides the core implementation of the use case execution.
    ///     This method must be implemented by derived classes.
    /// </summary>
    /// <param name="input">The input for the use case.</param>
    /// <returns>A Task that represents the asynchronous operation and yields the output.</returns>
    protected abstract Task<TOutput> ExecuteCoreAsync(TInput input);

    /// <summary>
    ///     Releases unmanaged resources used by the use case.
    ///     Derived classes can override this method to provide specific cleanup logic.
    /// </summary>
    /// <param name="disposing">
    ///     True if disposing is called from the Dispose method; otherwise, false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
    }
}

/// <summary>
///     Provides an abstract base class for use cases that take an input and do not produce an output.
///     This class implements the IUseCase&lt;TInput&gt; interface and provides
///     a default ExecuteAsync implementation.
/// </summary>
/// <typeparam name="TInput">The type of the input for the use case.</typeparam>
public abstract class BaseUseCase<TInput> : IUseCase<TInput>
{
    public IBackgroundThreadDispatcher BackgroundThreadDispatcher { get; set; }

    protected BaseUseCase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        BackgroundThreadDispatcher = ServiceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
    }

    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    ///     Executes the use case asynchronously.
    /// </summary>
    /// <param name="input">The input for the use case.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    public async Task ExecuteAsync(TInput input)
    {
        await BackgroundThreadDispatcher.InvokeAsync(() => ExecuteCoreAsync(input).ConfigureAwait(false));
    }

    /// <summary>
    ///     Disposes of any resources used by the use case.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Provides the core implementation of the use case execution.
    ///     This method must be implemented by derived classes.
    /// </summary>
    /// <param name="input">The input for the use case.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    protected abstract Task ExecuteCoreAsync(TInput input);

    /// <summary>
    ///     Releases unmanaged resources used by the use case.
    ///     Derived classes can override this method to provide specific cleanup logic.
    /// </summary>
    /// <param name="disposing">
    ///     True if disposing is called from the Dispose method; otherwise, false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
    }
}

/// <summary>
///     Provides an abstract base class for use cases that do not take any input and do not produce an output.
///     This class implements the IUseCase interface and provides a default ExecuteAsync implementation.
/// </summary>
public abstract class BaseUseCase : IUseCase
{
    protected BaseUseCase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        BackgroundThreadDispatcher = ServiceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
    }

    protected IBackgroundThreadDispatcher BackgroundThreadDispatcher { get; set; }

    protected IServiceProvider ServiceProvider { get; }

    /// <inheritdoc />
    public async Task ExecuteAsync()
    {
        await BackgroundThreadDispatcher.InvokeAsync(() => ExecuteCoreAsync().ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    /// <summary>
    ///     Disposes of any resources used by the use case.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Provides the core implementation of the use case execution.
    ///     This method must be implemented by derived classes.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    protected abstract Task ExecuteCoreAsync();

    /// <summary>
    ///     Releases unmanaged resources used by the use case.
    ///     Derived classes can override this method to provide specific cleanup logic.
    /// </summary>
    /// <param name="disposing">
    ///     True if disposing is called from the Dispose method; otherwise, false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
    }
}

/// <summary>
///     Provides an abstract base class for use cases that do not take any input but produce a result.
///     This class implements the IUseCaseWithResult&lt;TResult&gt; interface and provides
///     a default ExecuteAsync implementation.
/// </summary>
/// <typeparam name="TResult">The type of the result from the use case.</typeparam>
public abstract class BaseUseCaseWithResult<TResult> : IUseCaseWithResult<TResult>
{
    protected BaseUseCaseWithResult(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        BackgroundThreadDispatcher = ServiceProvider.GetRequiredService<IBackgroundThreadDispatcher>();
    }

    protected IBackgroundThreadDispatcher BackgroundThreadDispatcher { get; set; }

    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    ///     Disposes of any resources used by the use case.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Executes the use case asynchronously and returns a result.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation and yields the result.</returns>
    public async Task<TResult> ExecuteAsync()
    {
        return await BackgroundThreadDispatcher.InvokeTaskAsync(() => { return ExecuteCoreAsync(); })
            .ConfigureAwait(false);
    }

    /// <summary>
    ///     Provides the core implementation of the use case execution.
    ///     This method must be implemented by derived classes.
    /// </summary>
    /// <returns>A Task that represents the asynchronous operation and yields the result.</returns>
    protected abstract Task<TResult> ExecuteCoreAsync();

    /// <summary>
    ///     Releases unmanaged resources used by the use case.
    ///     Derived classes can override this method to provide specific cleanup logic.
    /// </summary>
    /// <param name="disposing">
    ///     True if disposing is called from the Dispose method; otherwise, false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
    }
}