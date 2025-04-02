using System.Collections.Concurrent;
using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Threading;

/// <summary>
/// Base dispatcher qui exécute des actions sur un thread dédié.
/// </summary>
public abstract class BaseThreadDispatcher : IThreadDispatcher, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Thread _dispatcherThread;
    private readonly ILogger<BaseThreadDispatcher> _logger;
    private readonly BlockingCollection<Action> _taskQueue = new();
    private bool _isDisposed;
    private bool _isStarted;

    /// <summary>
    /// Obtient une valeur indiquant si le dispatcher est en cours d'exécution.
    /// </summary>
    public bool IsRunning => _isStarted && !_isDisposed && _dispatcherThread.IsAlive;

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="BaseThreadDispatcher"/>.
    /// </summary>
    /// <param name="logger">Le logger à utiliser.</param>
    /// <param name="threadName">Le nom du thread dispatcher.</param>
    protected BaseThreadDispatcher(ILogger<BaseThreadDispatcher> logger, string threadName)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(threadName))
            threadName = "ThreadDispatcher";

        _dispatcherThread = new Thread(DispatcherThreadStart)
        {
            IsBackground = true,
            Name = threadName
        };
        _logger.LogInformation($"{threadName} Created.");
    }

    /// <summary>
    /// Libère les ressources utilisées par le dispatcher.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Exécute une action de manière synchrone sur le thread dispatcher.
    /// </summary>
    /// <param name="action">L'action à exécuter.</param>
    /// <exception cref="ObjectDisposedException">Le dispatcher a été libéré.</exception>
    /// <exception cref="InvalidOperationException">Le dispatcher n'a pas été démarré.</exception>
    /// <exception cref="AggregateException">Une exception s'est produite lors de l'exécution de l'action.</exception>
    public void Invoke(Action action)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, GetType());

        if (!IsRunning)
            throw new InvalidOperationException(
                "Thread dispatcher not started or already stopped. Call Start() first.");

        if (action == null)
            throw new ArgumentNullException(nameof(action));

        using var waitHandle = new ManualResetEventSlim(false);
        Exception capturedEx = null;

        _taskQueue.Add(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                capturedEx = ex;
            }
            finally
            {
                waitHandle.Set();
            }
        });

        if (!waitHandle.Wait(Timeout.Infinite, _cancellationTokenSource.Token))
            throw new OperationCanceledException(
                "The dispatcher was stopped while waiting for the action to complete.");

        if (capturedEx != null)
            throw new AggregateException("Exception thrown during execution", capturedEx);
    }

    /// <summary>
    /// Exécute une action de manière asynchrone sur le thread dispatcher.
    /// </summary>
    /// <param name="action">L'action à exécuter.</param>
    /// <returns>Une tâche représentant l'opération asynchrone.</returns>
    /// <exception cref="ObjectDisposedException">Le dispatcher a été libéré.</exception>
    /// <exception cref="InvalidOperationException">Le dispatcher n'a pas été démarré.</exception>
    public Task InvokeAsync(Action action)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, GetType());

        if (!IsRunning)
            throw new InvalidOperationException(
                "Thread dispatcher not started or already stopped. Call Start() first.");

        if (action == null)
            throw new ArgumentNullException(nameof(action));

        var tcs = new TaskCompletionSource<object>();
        _taskQueue.Add(() =>
        {
            try
            {
                action();
                tcs.SetResult(null);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task;
    }

    /// <summary>
    /// Exécute une fonction de manière asynchrone sur le thread dispatcher et retourne son résultat.
    /// </summary>
    /// <typeparam name="T">Le type du résultat.</typeparam>
    /// <param name="func">La fonction à exécuter.</param>
    /// <returns>Une tâche représentant l'opération asynchrone avec le résultat.</returns>
    /// <exception cref="ObjectDisposedException">Le dispatcher a été libéré.</exception>
    /// <exception cref="InvalidOperationException">Le dispatcher n'a pas été démarré.</exception>
    public Task<T> InvokeAsync<T>(Func<T> func)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, GetType());

        if (!IsRunning)
            throw new InvalidOperationException(
                "Thread dispatcher not started or already stopped. Call Start() first.");

        if (func == null)
            throw new ArgumentNullException(nameof(func));

        var tcs = new TaskCompletionSource<T>();
        _taskQueue.Add(() =>
        {
            try
            {
                var result = func();
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task;
    }

    /// <summary>
    /// Exécute une fonction asynchrone sur le thread dispatcher et retourne son résultat.
    /// </summary>
    /// <typeparam name="T">Le type du résultat.</typeparam>
    /// <param name="taskFunc">La fonction asynchrone à exécuter.</param>
    /// <returns>Une tâche représentant l'opération asynchrone avec le résultat.</returns>
    /// <exception cref="ObjectDisposedException">Le dispatcher a été libéré.</exception>
    /// <exception cref="InvalidOperationException">Le dispatcher n'a pas été démarré.</exception>
    public Task<T> InvokeTaskAsync<T>(Func<Task<T>> taskFunc)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, GetType());

        if (!IsRunning)
            throw new InvalidOperationException(
                "Thread dispatcher not started or already stopped. Call Start() first.");

        if (taskFunc == null)
            throw new ArgumentNullException(nameof(taskFunc));

        var tcs = new TaskCompletionSource<T>();
        _taskQueue.Add(async void () =>
        {
            try
            {
                var result = await taskFunc().ConfigureAwait(false);
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task;
    }

    /// <summary>
    /// Exécute une fonction asynchrone sur le thread dispatcher.
    /// </summary>
    /// <param name="taskFunc">La fonction asynchrone à exécuter.</param>
    /// <returns>Une tâche représentant l'opération asynchrone.</returns>
    /// <exception cref="ObjectDisposedException">Le dispatcher a été libéré.</exception>
    /// <exception cref="InvalidOperationException">Le dispatcher n'a pas été démarré.</exception>
    public Task InvokeTaskAsync(Func<Task> taskFunc)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, GetType());

        if (!IsRunning)
            throw new InvalidOperationException(
                "Thread dispatcher not started or already stopped. Call Start() first.");

        if (taskFunc == null)
            throw new ArgumentNullException(nameof(taskFunc));

        var tcs = new TaskCompletionSource<object>();
        _taskQueue.Add(async void () =>
        {
            try
            {
                await taskFunc().ConfigureAwait(false);
                tcs.SetResult(null);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        return tcs.Task;
    }

    /// <summary>
    /// Démarre le thread dispatcher.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Le dispatcher a été libéré.</exception>
    public void Start()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, GetType());

        if (!_isStarted)
        {
            _dispatcherThread.Start();
            _isStarted = true;
            _logger.LogInformation(
                $"{_dispatcherThread.Name} Started with ID {_dispatcherThread.ManagedThreadId}.");
        }
        else
        {
            _logger.LogWarning($"{_dispatcherThread.Name} already started.");
        }
    }

    /// <summary>
    /// Arrête le thread dispatcher.
    /// </summary>
    public void Stop()
    {
        if (_isStarted && !_isDisposed)
        {
            _logger.LogInformation(
                $"Stopping {_dispatcherThread.Name} - {_dispatcherThread.ManagedThreadId}.");

            try
            {
                _cancellationTokenSource.Cancel();

                // Ajouter une action vide pour débloquer TryTake si nécessaire
                if (!_taskQueue.IsAddingCompleted)
                    try
                    {
                        _taskQueue.Add(() => { });
                    }
                    catch (InvalidOperationException)
                    {
                        // La file d'attente pourrait être marquée comme complète entre la vérification et l'ajout
                    }

                // Attendre la fin du thread avec un timeout raisonnable
                var joined = _dispatcherThread.Join(TimeSpan.FromSeconds(5));
                if (!joined)
                    _logger.LogWarning(
                        $"{_dispatcherThread.Name} did not terminate gracefully within the timeout period.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error stopping {_dispatcherThread.Name}");
            }
            finally
            {
                _isStarted = false;
            }
        }
    }

    /// <summary>
    /// Méthode d'entrée du thread dispatcher.
    /// </summary>
    private void DispatcherThreadStart()
    {
        try
        {
            _logger.LogDebug($"{_dispatcherThread.Name} thread started.");

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    if (_taskQueue.TryTake(out var task, Timeout.Infinite, _cancellationTokenSource.Token))
                    {
                        if (task != null) task.Invoke();
                    }
                }
                catch (OperationCanceledException)
                {
                    // Graceful shutdown - break the loop
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception in dispatcher thread.");
                }
            }

            _logger.LogDebug($"{_dispatcherThread.Name} thread loop exited gracefully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fatal error in {_dispatcherThread.Name} thread.");
        }
    }

    /// <summary>
    /// Libère les ressources utilisées par le dispatcher.
    /// </summary>
    /// <param name="disposing">Indique si la méthode a été appelée depuis Dispose().</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
            try
            {
                Stop();

                try
                {
                    _taskQueue.CompleteAdding();
                }
                catch (ObjectDisposedException)
                {
                    // Ignorer si déjà libéré
                }

                _taskQueue.Dispose();
                _cancellationTokenSource.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during dispatcher disposal");
            }

        _isDisposed = true;
        _logger.LogInformation($"{_dispatcherThread.Name} Disposed");
    }
}