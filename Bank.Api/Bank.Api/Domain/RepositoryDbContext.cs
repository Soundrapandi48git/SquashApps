using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Domain
{
    public class RepositoryDbContext:DbContext
    {
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(options)
        {
        }
        public DbSet<AccountUserInfo> Accountinformation { get; set; }
        public DbSet<TransactionInfo> Transaction { get; set; }    
    }
}
