
namespace SparkUpSolution.Application.Requests
{
    public class PagedResult<T>
    {
        public PagedResult(IReadOnlyCollection<T> items, int pageNumber, int pageSize, int total)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = total;
        }

        public IReadOnlyCollection<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
