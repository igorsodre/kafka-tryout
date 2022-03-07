namespace Contracts.Requests.Queries;

public class PaginationQuery
{
    public PaginationQuery()
    {
        PageNumber = 1;
        PageSize = 25;
    }

    public PaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 100 ? 100 : pageSize;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}