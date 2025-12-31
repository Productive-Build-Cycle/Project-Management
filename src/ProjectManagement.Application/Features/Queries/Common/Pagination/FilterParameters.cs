using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.Features.Queries.Common.Pagination;

public class FilterParameters
{
    public int Page { get; set; }

    [Range(1, 1000)]
    public int PageSize { get; set; }

    public IList<Sort> SortColumns { get; set; } = new List<Sort>();

    public IList<FilterColumn> FilterColumn { get; set; } = new List<FilterColumn>();
}
#region SortAndFilter
public class Sort
{
    public string Column { get; set; }
    public SortDirection Direction { get; set; }
    public int OrderIndex { get; set; }
}
public class FilterColumn
{
    public string Column { get; set; }
    public IList<string> Values { get; set; }
    public FilterOperator Operator { get; set; }
}
#endregion
#region Enums
public enum SortDirection
{
    [Description("asc")]
    Accending,
    [Description("desc")]
    Dessending
}
public enum FilterOperator
{
    [Description("برابر")]
    Equal = 1,

    [Description("بزرگتر از")]
    GreaterThan = 2,

    [Description("کمتر از")]
    LessThan = 3,

    [Description("بزرگتر یا برابر")]
    GreaterOrEqual = 4,

    [Description("کمتر یا برابر")]
    LessOrEqual = 5,

    [Description("حاوی")]
    Contains = 6,

    [Description("شروع می شود با")]
    StartsWith = 7,

    [Description("به پایان می رسد با")]
    EndsWith = 8,

    [Description("بین")]
    Between = 9,

    [Description("نا برابر")]
    NotEqual = 10,

    [Description("شامل در لیست")]
    ContainsInList = 11,
}
#endregion