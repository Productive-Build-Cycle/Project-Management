using Mapster;
using ProjectManagement.Application.Features.Queries.Common.Pagination;
using System.Linq.Dynamic.Core;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Infrastructure.Common;

public static class QueryablePagedListExtensions
{
    private static readonly ParsingConfig ParsingConfig = new()
    {
        UseParameterizedNamesInDynamicQuery = false
    };

    public static async Task<PagedList<TDto>> ToPagedList<TEntity, TDto>(
        this IQueryable<TEntity> query,
        FilterParameters filter,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TDto : class, new()
    {
        if (filter is null)
            throw new ArgumentNullException(nameof(filter));

        filter.Page = filter.Page <= 0 ? 1 : filter.Page;
        filter.PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

        var (where, parameters) = filter.BuildWhere();
        if (!string.IsNullOrWhiteSpace(where))
            query = query.Where(ParsingConfig, where, parameters);

        var totalRecords = await query.LongCountAsync(cancellationToken);

        if (filter.SortColumns?.Any() == true)
            query = query.OrderBy(filter.BuildOrderBy());

        query = query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize);

        var rows = await query
            .ProjectToType<TDto>()
            .ToListAsync(cancellationToken);

        return new PagedList<TDto>(
            rows,
            filter.Page,
            filter.PageSize,
            totalRecords
        );
    }
    #region Helpers
    private static readonly Dictionary<FilterOperator, string> Operators = new()
    {
        { FilterOperator.Equal, "==" },
        { FilterOperator.NotEqual, "!=" },
        { FilterOperator.GreaterThan, ">" },
        { FilterOperator.GreaterOrEqual, ">=" },
        { FilterOperator.LessThan, "<" },
        { FilterOperator.LessOrEqual, "<=" }
    };

    private static (string, object[]) BuildWhere(this FilterParameters filter)
    {
        var sb = new StringBuilder();
        var parameters = new List<object>();
        var index = 0;

        foreach (var column in filter.FilterColumn)
        {
            if (column.Values == null || !column.Values.Any())
                continue;

            var paramName = $"@{index}";

            switch (column.Operator)
            {
                case FilterOperator.Contains:
                    sb.Append($"{column.Column}.Contains({paramName}) AND ");
                    parameters.Add(column.Values.First());
                    break;

                case FilterOperator.StartsWith:
                    sb.Append($"{column.Column}.StartsWith({paramName}) AND ");
                    parameters.Add(column.Values.First());
                    break;

                case FilterOperator.EndsWith:
                    sb.Append($"{column.Column}.EndsWith({paramName}) AND ");
                    parameters.Add(column.Values.First());
                    break;

                case FilterOperator.Between:
                    sb.Append($"{column.Column} >= @{index} AND {column.Column} <= @{index + 1} AND ");
                    parameters.Add(column.Values[0]);
                    parameters.Add(column.Values[1]);
                    index++;
                    break;

                default:
                    sb.Append($"{column.Column} {Operators[column.Operator]} {paramName} AND ");
                    parameters.Add(column.Values.First());
                    break;
            }

            index++;
        }

        if (sb.Length > 5)
            sb.Length -= 5; // remove last AND

        return (sb.ToString(), parameters.ToArray());
    }

    private static string BuildOrderBy(this FilterParameters filter)
    {
        return string.Join(",",
            filter.SortColumns
                .OrderBy(x => x.OrderIndex)
                .Select(x =>
                    $"{x.Column} {(x.Direction == SortDirection.Dessending ? "desc" : "asc")}"
                ));
    }
    #endregion
}