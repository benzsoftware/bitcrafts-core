namespace BitCrafts.Infrastructure.Abstraction.Threading;

/// <summary>
///     Defines an interface for retrieving optimal parallelism settings.
///     This is useful for configuring parallel operations based on the
///     capabilities of the underlying hardware.
/// </summary>
public interface IParallelism
{
    /// <summary>
    ///     Gets the optimal degree of parallelism based on whether the operation is CPU-bound.
    /// </summary>
    /// <param name="isCpuBound">
    ///     True if the operation is CPU-bound; otherwise, false.
    ///     CPU-bound operations benefit from a degree of parallelism close to the number of CPU cores,
    ///     while I/O-bound operations can benefit from a higher degree of parallelism.
    /// </param>
    /// <returns>The optimal degree of parallelism.</returns>
    int GetOptimalParallelism(bool isCpuBound = false);
}