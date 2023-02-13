using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamPlanner.Data;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ITimeRepository _timeRepository;
        private readonly IAccountRepository _accountRepository;
        public GroupRepository(ApplicationDbContext context, ITimeRepository timeRepository, IAccountRepository accountRepository)
        {
            _context = context;
            _timeRepository = timeRepository;
            _accountRepository = accountRepository;
        }
        public Group GetById(int id)
        {
            return _context.Groups.FirstOrDefault(g => g.Id == id);
        }
        public Group GetByName(string name)
        {
            return _context.Groups.FirstOrDefault(g => g.Name == name);
            
        }
        public List<Group> GetAll()
        {
            return _context.Groups.ToList();
        }
        public async Task<List<AccountSummary>> GetAllUsersByGroup(int groupId)
        {
            var entries = _context.GroupUsers.Where(item => item.GroupId == groupId).ToList();
            List<AccountSummary> userList = new List<AccountSummary>();
            foreach(var entry in entries)
            {
                var user = await _accountRepository.GetById(entry.WorkerId);
                var time = await _timeRepository.GetHourSummary(user.Id, groupId);
                var unavTime = await _timeRepository.GetUnavailableHourSummary(user.Id, groupId);
                var summary = new AccountSummary(user)
                {
                    HoursSummary = time,
                    UnavailableHoursSummary = unavTime,
                };
                userList.Add(summary);
            }
            return userList;
        }
        public async Task<List<AccountSummary>> GetAllUsersByGroup(int groupId, string week)
        {
            var entries = _context.GroupUsers.Where(item => item.GroupId == groupId).ToList();
            List<AccountSummary> userList = new List<AccountSummary>();

            foreach (var entry in entries)
            {
                var user = await _accountRepository.GetById(entry.WorkerId);
                var time = await _timeRepository.GetHourSummaryByWeek(week, user.Id, groupId);
                var unavTime = await _timeRepository.GetUnavailableHourSummaryByWeek(week, user.Id, groupId);
                var summary = new AccountSummary(user)
                {
                    HoursSummary = time,
                    UnavailableHoursSummary = unavTime
                };
                userList.Add(summary);
            }
            return userList;
        }
        public async Task<IEnumerable<Group>> GetAllGroupsByUser(string uid)
        {
            var entries = _context.GroupUsers.Where(item => item.WorkerId == uid).ToList();
            List<Group> groups = new List<Group>();
            foreach(var entry in entries)
            {
                var group = await _context.Groups.FirstOrDefaultAsync(item => item.Id == entry.GroupId);
                groups.Add(group);
            }
            return groups;
        }
        public bool Create(Group group)
        {
            _context.Groups.Add(group);
            return Save();
        }
        public bool CreateLink(GroupUser groupUser)
        {
            var groupCheck = _context.GroupUsers.Where(item => item.GroupId == groupUser.GroupId && item.WorkerId == groupUser.WorkerId).ToList();
            if(groupCheck.Count > 0)
            {
                return false;
            }
            _context.GroupUsers.Add(groupUser);
            return Save();
        }
        public bool Update(Group group)
        {
            _context.Groups.Update(group);
            return Save();
        }
        public bool Delete(Group group)
        {
            _context.Groups.Remove(group);
            return Save();
        }
        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }
    }
}
