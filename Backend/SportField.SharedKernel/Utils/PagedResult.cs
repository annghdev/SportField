namespace SportField.SharedKernel.Utils
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNext => PageNumber < TotalPages;
        public bool HasPrevious => PageNumber > 1;
        public IEnumerable<T>? Items { get; set; }
        public PagedResult(int pageNumber, int pageSize, int totalCount, IEnumerable<T>? items)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items;
        }
    }
}
