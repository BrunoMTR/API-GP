using System;

namespace Domain.Results
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

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
        public static Response<T> Ok(T data, string message = "")
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }
    }
}
