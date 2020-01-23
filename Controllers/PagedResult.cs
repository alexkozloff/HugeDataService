namespace HugeDataService.Controllers
{
    public class PagedResult<T>
    {
        public PagedResult(T data, long totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public T Data { get; }
        public long TotalCount { get; }
    }
}