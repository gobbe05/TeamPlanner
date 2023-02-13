using Microsoft.EntityFrameworkCore;
using TeamPlanner.Data;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITimeRepository _timeRepository;

        public AccountRepository(ApplicationDbContext context, ITimeRepository timeRepository)
        {
            _context = context;
            _timeRepository = timeRepository;
        }
        public async Task<List<Account>> GetAll()
        {
            return await _context.Accounts.ToListAsync();
        }
        public async Task<Account> GetById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<Account> GetByIdAsyncNoTracking(string id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<bool> Accept(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(item => item.UserName == username);
            if(user != null)
            {
                user.Accepted = true;
            }
            _context.SaveChanges();
            return true;

        }
        public async Task<UserSummary?> GetUserSummaryByWeek(string week, string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(item => item.Id == id);
            if (user == null) return null;

            int hourSummary = await _timeRepository.GetHourSummaryByWeek(week, id);
            int unavHourSummary = await _timeRepository.GetUnavailableHourSummaryByWeek(week, id);
            int earned = hourSummary * user.Salary;
            UserSummary summary = new UserSummary
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Hours = hourSummary,
                Absent = unavHourSummary,
                Earned= earned

            };
            return summary;
        }
        public bool Update(Account account)
        {
            _context.Update(account);
            return Save();
        }
        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }
    }
}
