namespace Streetcode.DAL.Entities.Base
{
    public interface IEntityId<T> : IEntity
    {
        public T Id { get; set; }
    }
}
