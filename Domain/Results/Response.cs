using System;
using System.Text.Json.Serialization;

namespace Domain.Results
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalCount { get; set; } // opcional, só aparece se tiver valor

        // Método estático para falha
        public static Response<T> Fail(string message)
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

        // Método estático para sucesso
        public static Response<T> Ok(T data, string message = "", int? totalCount = null)
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TotalCount = totalCount
            };
        }
    }
}
