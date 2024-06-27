using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Instagram
{
    public class InstagramPostResponse : IEntity
    {
        public IEnumerable<InstagramPost>? Data { get; set; }
    }
}
