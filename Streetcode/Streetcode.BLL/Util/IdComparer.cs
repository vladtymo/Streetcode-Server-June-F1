using System.Diagnostics.CodeAnalysis;
using Streetcode.DAL.Entities.Base;

namespace Streetcode.BLL.Util
{
    public class IdComparer<T> : IEqualityComparer<T> 
        where T : IEntityId<int>
    {
        public bool Equals(T? x, T? y)
            {
            if(x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.Id.GetHashCode();
        } 
    }
}