namespace AccountService.DTOs
{
    /// <summary>
    /// Data transfer object for response of account list.
    /// </summary>
    public class AccountResponseDto : BaseResponseDto
    {
        public List<AccountInfo> Data { get; set; }
    }

    public class AccountInfo
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
