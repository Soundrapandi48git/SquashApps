using Domain;
using Domain.DataTransferObjects;
using Domain.Entities;
using Infrastructure.IServices;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository
{
    public class AccountRepository : RepositoryBase<AccountUserInfo>, IAccountRepository
    {
        private readonly RepositoryDbContext _dbContext;
        public AccountRepository(RepositoryDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(AccountUserInfo entity)
        {
            await Create(entity);
        }

        public  void DeleteAsync(AccountUserInfo entity)
        {
             Delete(entity);
        }

        public async Task<IEnumerable<AccountUserInfo>> GetAllAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<AccountUserInfo> GetByIdAsync(int id)
        {
           return await FindByCondition(account => account.UserId.Equals(id)).FirstOrDefaultAsync();
        }

        public void UpdateAsync(AccountUserInfo entity)
        {
             Update(entity);
        }
    }
}
