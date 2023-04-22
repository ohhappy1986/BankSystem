namespace AccountService.DTOs
{
    /// <summary>
    /// Base data transfer object for response.
    /// </summary>
    public abstract class BaseResponseDto
    {
        public int ResponseCode { get; set; }

        public string? Message { get; set; }
    }
}
