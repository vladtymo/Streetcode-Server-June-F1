using System.Linq.Expressions;
using System.Reflection;
using Ardalis.Specification;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Specification.Streetcode.Streetcode.GetAll;

public class StreetcodesSortedSpec : Specification<StreetcodeContent>
{
    public StreetcodesSortedSpec(string sort)
    {
        if (!string.IsNullOrEmpty(sort))
        {
            ApplySorting(sort);
        }
    }

    private void ApplySorting(string sort)
    {
        var sortColumn = sort.Trim();
        var sortDirection = "asc";

        if (sortColumn.StartsWith('-'))
        {
            sortDirection = "desc";
            sortColumn = sortColumn.Substring(1);
        }

        var property = GetProperty(sortColumn);
        if (property == null)
        {
            return;
        }

        var orderByExpression = GetOrderByExpression(property);
        ApplyOrderBy(orderByExpression, sortDirection);
    }

    private PropertyInfo? GetProperty(string sortColumn)
    {
        var type = typeof(StreetcodeContent);
        return type.GetProperty(sortColumn);
    }

    private Expression<Func<StreetcodeContent, object?>> GetOrderByExpression(PropertyInfo property)
    {
        var parameter = Expression.Parameter(typeof(StreetcodeContent), "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        return Expression.Lambda<Func<StreetcodeContent, object?>>(Expression.Convert(propertyAccess, typeof(object)), parameter);
    }

    private void ApplyOrderBy(Expression<Func<StreetcodeContent, object?>> orderByExpression, string sortDirection)
    {
        if (sortDirection == "asc")
        {
            Query.OrderBy(orderByExpression);
        }
        else
        {
            Query.OrderByDescending(orderByExpression);
        }
    }
}
