using System.Text.Json.Serialization;

namespace Dima.Core.Responses
{
    public class PagedResponse<TData> : Response<TData> where TData : class
    {
        [JsonConstructor]
        public PagedResponse(
            TData? data,
            int totalCount,
            int currentPage = 1,
            int pageSize = Configurations.PAGE_SIZE)
            : base(data)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public PagedResponse(
            TData? data,
            int code = Configurations.DEFAULT_STATUS_CODE,
            string? message = null)
            : base(data, code, message)
        {

        }

        public int CurrentPage { get; set; } = 1;
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)PageSize);
            }
        }

        public int PageSize { get; set; } = Configurations.PAGE_SIZE;
        public int TotalCount { get; set; }
    }
}
