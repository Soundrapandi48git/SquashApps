using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities
{
    [Table("Transaction")]
    public class TransactionInfo
    {
        [Key]
        public int TransactionNumber { get; set; }
        public string AccountNumber { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
