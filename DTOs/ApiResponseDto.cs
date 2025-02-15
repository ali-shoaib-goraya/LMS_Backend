namespace Dynamic_RBAMS.DTOs
{
    // ApiResponse.cs
    public class ApiResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ApiResponseDto(int statusCode, string message, object? data = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data ;
        }
    }
}
 