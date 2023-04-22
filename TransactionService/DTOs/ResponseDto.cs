namespace TransactionService.DTOs
{
    /// <summary>
    /// General data transfer object for response
    /// </summary>
    public class ResponseDto : BaseResponseDto
    {
        public ResponseDto() { }

        public ResponseDto(string? message, int responseCode)
        {
            Message = message;
            ResponseCode = responseCode;
        }
    }
}
