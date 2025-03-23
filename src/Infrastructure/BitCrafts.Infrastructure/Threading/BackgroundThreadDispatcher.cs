using BitCrafts.Infrastructure.Abstraction.Threading;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Threading;

public sealed class BackgroundThreadDispatcher : BaseThreadDispatcher, IBackgroundThreadDispatcher
{
    public BackgroundThreadDispatcher(ILogger<BackgroundThreadDispatcher> logger)
        : base(logger, "Background Thread Dispatcher")
    {
    }
}