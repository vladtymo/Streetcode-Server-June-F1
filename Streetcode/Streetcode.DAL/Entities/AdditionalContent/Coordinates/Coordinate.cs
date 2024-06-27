using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.AdditionalContent.Coordinates;

public class Coordinate : IEntityId<int>
{
    public int Id { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longtitude { get; set; }
}