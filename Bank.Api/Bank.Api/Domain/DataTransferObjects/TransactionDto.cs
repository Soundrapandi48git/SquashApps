namespace Domain.DataTransferObjects
{
    public class TransactionDto
    {
        public int UserID { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
    }
}
