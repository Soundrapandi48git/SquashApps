using Domain;
using Domain.Entities;
using Infrastructure.IServices;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository
{
    public class TransRepository : RepositoryBase<TransactionInfo>, ITransactionRepository
    {
        private readonly RepositoryDbContext _dbContext;
        public TransRepository(RepositoryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(TransactionInfo entity)
        {
            await Create(entity);
        }

        public async Task<IEnumerable<TransactionInfo>> GetAllAsync()
        {
          return  await FindAll().ToListAsync();
        }

        public async  Task<TransactionInfo> GetByIdAsync(int id)
        {
            return await FindByCondition(transction => transction.TransactionNumber.Equals(id)).FirstOrDefaultAsync();
        }
    }
}
