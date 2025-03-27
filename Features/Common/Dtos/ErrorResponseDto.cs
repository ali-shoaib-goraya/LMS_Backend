namespace LMS.Features.Common.Dtos
{
    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
