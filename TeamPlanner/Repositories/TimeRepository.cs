using Microsoft.EntityFrameworkCore;
using TeamPlanner.Data;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;

namespace TeamPlanner.Repositories
{
    public class TimeRepository : ITimeRepository
    {
        private readonly ApplicationDbContext _context;
        public TimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateTime(Time time)
        {
            var group = _context.Groups.FirstOrDefault(group => group.Id == time.GroupId);
            if (group == null)
            {
                return false;
            }
            int timeStart = Int32.Parse(time.Start.Split('.')[0]);
            int timeEnd = Int32.Parse(time.End.Split('.')[0]);
            group.MoneyUsed += time.Salary * (timeEnd - timeStart);
            group.TimeUsed += timeEnd - timeStart;
            _context.Groups.Update(group);
            _context.Times.Add(time);
            return Save();
        }
        public bool CreateUnavailableTime(UnavailableTime time)
        {
            _context.UnavailableTimes.Add(time);
            return Save();
        }
        public async Task<List<Time>> GetAllTimesByWeekAsync(string week, string id)
        {
            return await _context.Times.Where(item => item.Week == week && item.WorkerId == id).ToListAsync();
        }
        public async Task<List<UnavailableTime>> GetAllUnavailableTimesByWeekAsync(string week, string id)
        {
            return await _context.UnavailableTimes.Where(item => item.Week == week && item.WorkerId == id).ToListAsync();
        }
        public async Task<int> GetHourSummaryByWeek(string week, string id)
        {
            var sum = 0;
            var times = await _context.Times.Where(item => item.Week == week && item.WorkerId == id).ToListAsync();
            foreach (var time in times)
            {
                TimeSpan duration = DateTime.Parse(time.End.Replace('.', ':')).Subtract(DateTime.Parse(time.Start.Replace('.', ':')));
                sum += (int)duration.TotalMinutes;
            }
            return sum / 60;
        }
        public async Task<int> GetHourSummaryByWeek(string week, string id, int groupId) 
        {
            var sum = 0;
            var times = await _context.Times.Where(item => item.Week == week && item.WorkerId == id && item.GroupId == groupId).ToListAsync();
            foreach (var time in times)
            {
                TimeSpan duration = DateTime.Parse(time.End.Replace('.', ':')).Subtract(DateTime.Parse(time.Start.Replace('.', ':')));
                sum += (int)duration.TotalMinutes;
            }
            return sum/60;
        }
        public async Task<int> GetHourSummary(string id, int groupId)
        {
            var sum = 0;
            var times = await _context.Times.Where(item => item.WorkerId == id && item.GroupId == groupId).ToListAsync();
            foreach (var time in times)
            {
                TimeSpan duration = DateTime.Parse(time.End.Replace('.', ':')).Subtract(DateTime.Parse(time.Start.Replace('.', ':')));
                sum += (int)duration.TotalMinutes;
            }
            return sum / 60;
        }
        public async Task<int> GetUnavailableHourSummaryByWeek(string week, string id)
        {
            var sum = 0;
            var times = await _context.UnavailableTimes.Where(item => item.Week == week && item.WorkerId == id).ToListAsync();
            foreach (var time in times)
            {
                TimeSpan duration = DateTime.Parse(time.End.Replace('.', ':')).Subtract(DateTime.Parse(time.Start.Replace('.', ':')));
                sum += (int)duration.TotalMinutes;
            }
            return sum / 60;
        }
        public async Task<int> GetUnavailableHourSummaryByWeek(string week, string id, int groupId)
        {
            var sum = 0;
            var times = await _context.UnavailableTimes.Where(item => item.Week == week && item.WorkerId == id && item.GroupId == groupId).ToListAsync();
            foreach (var time in times)
            {
                TimeSpan duration = DateTime.Parse(time.End.Replace('.', ':')).Subtract(DateTime.Parse(time.Start.Replace('.', ':')));
                sum += (int)duration.TotalMinutes;
            }
            return sum / 60;
        }
        public async Task<int> GetUnavailableHourSummary(string id, int groupId)
        {
            var sum = 0;
            var times = await _context.UnavailableTimes.Where(item => item.WorkerId == id && item.GroupId == groupId).ToListAsync();
            foreach (var time in times)
            {
                TimeSpan duration = DateTime.Parse(time.End.Replace('.', ':')).Subtract(DateTime.Parse(time.Start.Replace('.', ':')));
                sum += (int)duration.TotalMinutes;
            }
            return sum / 60;
        }
        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0;
        }
    }
}
