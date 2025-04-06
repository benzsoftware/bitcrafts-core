using Avalonia.Headless;

namespace BitCrafts.Application.Avalonia.Tests;

public static class AvaloniaTestHelper
{
    /// <summary>
    /// Méthode d'extension pour exécuter du code sur le thread UI et attendre sa complétion
    /// </summary>
    public static async Task RunOnUIThread(this HeadlessUnitTestSession session, Action action)
    {
        await session.Dispatch(() =>
        {
            action();
            return true;
        }, CancellationToken.None);
    }

    /// <summary>
    /// Méthode d'extension pour exécuter du code sur le thread UI qui retourne une valeur
    /// </summary>
    public static async Task<T> RunOnUIThread<T>(this HeadlessUnitTestSession session, Func<T> function)
    {
        return await session.Dispatch(function, CancellationToken.None);
    }
}