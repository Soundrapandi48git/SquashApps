using Domain;
using Domain.DataTransferObjects;
using Infrastructure.IServices;

namespace Application.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly RepositoryDbContext _dbContext;
        private  IAccountRepository _accountRepository;
        private  ITransactionRepository _transactionRepository;  
        public RepositoryWrapper(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_dbContext);
                }
                return _accountRepository;
            }
        }
        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                {
                    _transactionRepository = new TransRepository(_dbContext);
                }
                return _transactionRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
