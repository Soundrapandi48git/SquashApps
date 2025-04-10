using Domain.Entities;

namespace Infrastructure.IServices
{
    public interface IAccountRepository
    {
        Task<AccountUserInfo> GetByIdAsync(int id);
        Task<IEnumerable<AccountUserInfo>> GetAllAsync();
        Task CreateAsync(AccountUserInfo entity);
        void UpdateAsync(AccountUserInfo entity);
        void DeleteAsync(AccountUserInfo entity);
    }
}
