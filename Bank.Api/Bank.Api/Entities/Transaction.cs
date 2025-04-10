using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int TransactionNumber { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
