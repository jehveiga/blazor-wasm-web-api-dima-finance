using System.Text.Json.Serialization;

namespace Dima.Core.Requests.Categories
{
    public class GetCategoryByIdRequest : Request
    {
        [JsonIgnore]
        public long Id { get; set; }
    }
}
