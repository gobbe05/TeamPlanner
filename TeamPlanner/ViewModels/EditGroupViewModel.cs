using TeamPlanner.Models;

namespace TeamPlanner.ViewModels
{
    public class EditGroupViewModel
    {
        public Group group { get; set; }
        public List<AccountSummary> Users { get; set; }
    }
}
