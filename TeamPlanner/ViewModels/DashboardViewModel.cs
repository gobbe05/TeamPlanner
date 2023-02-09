using TeamPlanner.Models;

namespace TeamPlanner.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<Time> Times { get; set; }
        public IEnumerable<UnavailableTime> UnavailableTimes { get; set; }
        public IEnumerable<Group> Groups { get; set; }
    }
}
