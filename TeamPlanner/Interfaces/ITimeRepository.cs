using TeamPlanner.Models;

namespace TeamPlanner.Interfaces
{
    public interface ITimeRepository
    {
        bool CreateTime(Time time);
        bool CreateUnavailableTime(UnavailableTime time);
        Task<List<Time>> GetAllTimesByWeekAsync(string week, string id);
        Task<List<UnavailableTime>> GetAllUnavailableTimesByWeekAsync(string week, string id);
        Task<int> GetHourSummaryByWeek(string week, string id, int groupId);
        Task<int> GetHourSummary(string id, int groupId);
        Task<int> GetUnavailableHourSummaryByWeek(string week, string id, int groupId);
        Task<int> GetUnavailableHourSummary(string id, int groupId);
        bool Save();
    }
}
