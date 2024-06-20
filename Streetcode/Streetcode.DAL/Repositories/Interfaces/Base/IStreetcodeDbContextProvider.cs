using Microsoft.Extensions.Caching.Distributed;
using Streetcode.DAL.Persistence;

namespace Streetcode.DAL.Repositories.Interfaces.Base
{
    public interface IStreetcodeDbContextProvider
    {
        StreetcodeDbContext DbContext { init; }
    }

    public interface IReddisDistributedCacheProvider
    { 
        IDistributedCache DistributedCache { init; }
    }    
}
