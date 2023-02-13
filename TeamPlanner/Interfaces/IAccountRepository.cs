using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAll();
        Task<Account> GetById(string id);
        Task<Account> GetByIdAsyncNoTracking(string id);
        Task<bool> Accept(string username);
        Task<UserSummary?> GetUserSummaryByWeek(string week, string id);
        //Task<UserSummary?> GetUserSummaryByMonth(string week, string id);
        bool Update(Account account);
        bool Save();
    }
}
