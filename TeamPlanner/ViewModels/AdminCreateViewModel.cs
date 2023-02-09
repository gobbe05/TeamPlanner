using TeamPlanner.Models;

namespace TeamPlanner.ViewModels
{
    public class AdminCreateViewModel
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Week { get; set; }
        public string Weekday { get; set; }
        public string Username { get; set; }
        public List<Group>? Groups { get; set; }
        public int GroupId { get; set; }
    }
}
