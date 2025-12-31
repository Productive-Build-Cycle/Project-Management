namespace ProjectManagement.Application.Features.Queries.Common.Pagination;

public class PagedList<TModel> where TModel : class, new()
{
    public PagedList()
    { }

    public PagedList(IList<TModel> rows, int currentPage, int pageCount, long totalRecords)
    {
        Rows = rows;
        CurrentPage = currentPage;
        PageCount = pageCount;
        TotalRecords = totalRecords;
    }

    public long TotalRecords { get; set; }
    public int PageCount { get; set; }
    public int CurrentPage { get; set; }
    public IList<TModel> Rows { get; set; }
    //public IList<AggregateColumn> AggregateColumns { get; set; }
}
/*
public class AggregateColumn
{
    public string Column { get; set; }

    public long PreviewsPageAggregate { get; set; }
    public long CurrentPageAggregate { get; set; }
    public long NextPageAggregate { get; set; }
    public long TotalAggregation { get; set; }
}
*/