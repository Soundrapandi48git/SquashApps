using Domain;
using Domain.Entities;
using Infrastructure.IServices;

namespace Application.Repository
{
    public class TransactionRepository:RepositoryBase<TransactionInfo>, ITransactionRepository
    {
        private readonly RepositoryDbContext _dbContext;
        public TransactionRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task CreateAsync(TransactionInfo entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TransactionInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TransactionInfo> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TransactionInfo entity)
        {
            throw new NotImplementedException();
        }
    }
}
