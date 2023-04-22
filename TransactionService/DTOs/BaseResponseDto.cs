using Newtonsoft.Json;

namespace TransactionService.DTOs
{
    /// <summary>
    /// Base data transfer object for response.
    /// </summary>
    public abstract class BaseResponseDto
    {
        [JsonProperty("responseCode")]
        public int ResponseCode { get; set; }
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
