using TeamPlanner.Models;

namespace TeamPlanner.ViewModels
{
    public class OverviewViewModel
    {
        public List<Time> Times { get; set; }
        public List<UnavailableTime> UnavailableTimes { get; set; }
        public string? Week { get; set; }
    }
}
