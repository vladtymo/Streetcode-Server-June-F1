using Streetcode.DAL.Persistence;

namespace Streetcode.DAL.Repositories.Interfaces.Base
{
    public interface IStreetcodeDbConextPropertyProvider
    {
        StreetcodeDbContext DbContext { init; }
    }
}
