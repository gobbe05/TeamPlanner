using System.ComponentModel.DataAnnotations;
using TeamPlanner.Models;

namespace TeamPlanner.ViewModels
{
    public class GroupDetailsViewModel
    {
        [Required]
        public Group Group { get; set; }
        public IEnumerable<AccountSummary> Accounts { get; set; }
        public string? Week { get; set; }
    }
}
