namespace Dima.Core.Requests
{
    public abstract class PagedRequest : Request
    {
        public int PageNumber { get; set; } = Configurations.PAGE_NUMBER;
        public int PageSize { get; set; } = Configurations.PAGE_SIZE;
    }
}
