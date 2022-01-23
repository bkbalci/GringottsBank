using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GringottsBank.Entities.DTO.Shared
{
    public class ApiResponse<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        public bool IsSuccessful { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; }

        public static ApiResponse<T> Success(int statusCode, T data)
        {
            return new ApiResponse<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static ApiResponse<T> Success(int statusCode)
        {
            return new ApiResponse<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static ApiResponse<T> Fail(int statusCode, List<string> errors)
        {
            return new ApiResponse<T> { StatusCode = statusCode, IsSuccessful = false, Errors = errors };
        }

        public static ApiResponse<T> Fail(int statusCode, string error)
        {
            return new ApiResponse<T> { StatusCode = statusCode, IsSuccessful = false, Errors = new List<string> { error } };
        }
    }
}
