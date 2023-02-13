using TeamPlanner.Models;

namespace TeamPlanner.ViewModels
{

    public class GroupSummary
    {
        public string Name { get; set; }
        public int Hours { get; set; }
    }
    public class DashboardViewModel
    {
        public int Week { get; set; }
        public UserSummary UserSummary { get; set; }
        public IEnumerable<Time> Times { get; set; }
        public IEnumerable<UnavailableTime> UnavailableTimes { get; set; }
        public IEnumerable<GroupSummary> Groups { get; set; }
    }
}
