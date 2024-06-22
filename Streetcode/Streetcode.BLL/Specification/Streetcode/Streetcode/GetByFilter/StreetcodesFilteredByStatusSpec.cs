using Ardalis.Specification;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Specification.Streetcode.Streetcode.GetByFilter;

public class StreetcodesFilteredByStatusSpec : Specification<StreetcodeContent>
{
    public StreetcodesFilteredByStatusSpec(string filter)
    {
        var filterParams = filter.Split(':');
        if (filterParams.Length == 2)
        {
            var filterValue = filterParams[1];
            if (!string.IsNullOrEmpty(filterValue))
            {
                Query.Where(s => filterValue.Contains(s.Status.ToString()));
            }
        }
    }
}
