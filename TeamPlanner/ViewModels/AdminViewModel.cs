using TeamPlanner.Models;

namespace TeamPlanner.ViewModels
{
    public class AdminViewModel
    {
        public List<Time> Times { get; set; }
        public List<UnavailableTime> UnavailableTimes { get; set; }
        public string? Week { get; set; }
        public string? Username { get; set; }
    }
}
