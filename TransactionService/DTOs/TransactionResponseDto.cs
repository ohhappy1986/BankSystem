namespace TransactionService.DTOs
{
    /// <summary>
    /// Data transfer object for Transactions list response.
    /// </summary>
    public class TransactionResponseDto : BaseResponseDto
    {
        public List<TransactionInfo> Data { get; set; }
    }

    public class TransactionInfo
    {
        public int AccountId { get; set; }
        public string TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
