using Streetcode.DAL.Persistence;

namespace Streetcode.DAL.Repositories.Interfaces.Base
{
    public interface IStreetcodeDbConextProvider
    {
        StreetcodeDbContext DbContext { init; }
    }
}
