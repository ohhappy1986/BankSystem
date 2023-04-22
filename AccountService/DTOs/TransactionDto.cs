namespace AccountService.DTOs
{
    /// <summary>
    /// Data transfer object for Transaction
    /// </summary>
    public class TransactionDto
    {
        public int AccountId { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
