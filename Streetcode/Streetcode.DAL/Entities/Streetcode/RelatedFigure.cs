using System.ComponentModel.DataAnnotations;
using Streetcode.DAL.Entities.Base;

namespace Streetcode.DAL.Entities.Streetcode;

public class RelatedFigure : IEntity
{
    public int ObserverId { get; set; }

    public StreetcodeContent Observer { get; set; }

    public int TargetId { get; set; }

    public StreetcodeContent Target { get; set; }
}