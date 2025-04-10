using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace Domain.Entities
{
    [Table("Accountinformation")]
    public class AccountUserInfo
    {
        [Key]
        public int UserId { get; set; }
        public string ?Username { get; set; }
        public string? Password { get; set; }
        public string ?AccountNumber { get; set; }
        public string ?Email { get; set; }
        public string ?PhoneNumber { get; set; }
        public DateTime RegisteredOn { get; set; }
        public bool IsActive { get; set; }
        public string ?CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Precision(18, 2)]
        public decimal Balance { get; set; }
    }
}
