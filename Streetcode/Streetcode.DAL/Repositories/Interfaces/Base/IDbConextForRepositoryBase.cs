using Streetcode.DAL.Persistence;

namespace Streetcode.DAL.Repositories.Interfaces.Base
{
    public interface IDbConextForRepositoryBase
    {
        StreetcodeDbContext DbContext { set; }
    }
}
