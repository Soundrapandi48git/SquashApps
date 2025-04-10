using Domain.DataTransferObjects;
using Domain.Entities;

namespace Infrastructure.IServices
{
    public interface ITransactionRepository
    {
        Task<TransactionInfo> GetByIdAsync(int id);
        Task<IEnumerable<TransactionInfo>> GetAllAsync();
        Task CreateAsync(TransactionInfo entity);
    }
}
