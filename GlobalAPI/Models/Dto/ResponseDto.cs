namespace GlobalAPI.Models.Dto
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? Error { get; set; }
        public string? RequestUrl { get; set; } // Optional: helpful for debugging
    }
}
