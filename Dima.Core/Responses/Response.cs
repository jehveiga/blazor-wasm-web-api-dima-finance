﻿using System.Text.Json.Serialization;

namespace Dima.Core.Responses
{
    public class Response<TData>
    {
        private readonly int _code;

        [JsonConstructor]
        public Response() =>
            _code = Configurations.DEFAULT_STATUS_CODE;

        public Response(
            TData? data,
            int code = Configurations.DEFAULT_STATUS_CODE,
            string? message = default)
        {
            Data = data;
            Message = message;
            _code = code;
        }

        public TData? Data { get; set; }
        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess =>
            _code is >= 200 and <= 299;
    }
}