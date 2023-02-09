using Microsoft.EntityFrameworkCore;
using TeamPlanner.Data;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;

namespace TeamPlanner.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
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
