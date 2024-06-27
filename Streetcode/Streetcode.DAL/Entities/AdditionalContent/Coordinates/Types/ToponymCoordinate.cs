using Streetcode.DAL.Entities.Base;
using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;

public class ToponymCoordinate : Coordinate, IEntity
{
    public int ToponymId { get; set; }

    public Toponym? Toponym { get; set; }
}