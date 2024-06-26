namespace Streetcode.DAL.Entities.Base
{
    internal interface IEntityId : IEntity
    {
        public int Id { get; set; }
    }
}
