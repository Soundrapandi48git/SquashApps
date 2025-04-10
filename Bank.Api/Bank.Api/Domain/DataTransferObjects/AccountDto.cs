namespace Domain.DataTransferObjects
{
    public class AccountDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string? AccountNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegisteredOn { get; set; }
        public bool IsActive { get; set; }
        public decimal? Balance { get; set; }
    }
}
