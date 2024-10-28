using System.Text.Json.Serialization;

namespace Dima.Core.Responses
{
    public class Response<TData> where TData : class
    {

        private readonly int _code;

        [JsonConstructor]
        public Response() =>
            _code = Configurations.DEFAULT_STATUS_CODE;


        public Response(
            TData? data,
            int code = Configurations.DEFAULT_STATUS_CODE,
            string? message = null)
        {
            Data = data;
            Message = message;
            _code = code;
        }

        public TData? Data { get; set; }
        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSucess =>
            _code is >= 200 and <= 299;
    }
}
