namespace AccountService.DTOs
{
    /// <summary>
    /// Data transfer object for account request
    /// </summary>
    public class AccountDto
    {
        public int UserId { get; set; }
        public decimal StartingBalance { get; set; }
    }
}
