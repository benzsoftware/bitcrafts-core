using BitCrafts.Infrastructure.Abstraction.Threading;

namespace BitCrafts.Infrastructure.Threading;

public sealed class Parallelism : IParallelism
{
    public int GetOptimalParallelism(bool isCpuBound = false)
    {
        var processorCount = Environment.ProcessorCount;

        if (isCpuBound) return Math.Max(1, processorCount - 1);

        return processorCount * 2;
    }
}