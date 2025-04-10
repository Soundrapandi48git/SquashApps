namespace Infrastructure.IServices
{
    public interface IRepositoryWrapper
    {
        IAccountRepository AccountRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        Task SaveAsync();
    }
}
