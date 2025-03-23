using BitCrafts.Infrastructure.Abstraction.Modules;

namespace BitCrafts.Infrastructure.Application.Events;

public class MenuClickEventArgs
{
    public IModule Module { get; set; }
}