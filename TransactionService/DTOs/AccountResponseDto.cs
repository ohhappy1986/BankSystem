using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TransactionService.DTOs
{
    /// <summary>
    /// Data transfer object for response of account list.
    /// </summary>
    public class AccountResponseDto : BaseResponseDto
    {
        [JsonProperty("data")]
        public List<AccountInfo> Data { get; set; }
    }

    public class AccountInfo
    {
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
    }
}
