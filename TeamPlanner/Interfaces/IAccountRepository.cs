using TeamPlanner.Models;

namespace TeamPlanner.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAll();
        Task<Account> GetById(string id);
        Task<Account> GetByIdAsyncNoTracking(string id);
        Task<bool> Accept(string username);
        bool Update(Account account);
        bool Save();
    }
}
