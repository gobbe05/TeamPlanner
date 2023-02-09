using TeamPlanner.Models;

namespace TeamPlanner.ViewModels
{
    public class AccountSummary : Account
    {
        public AccountSummary(Account account)
        {
            FirstName = account.FirstName;
            LastName = account.LastName;
            UserName = account.UserName;
            Email = account.Email;
        }
        public int HoursSummary { get; set; }
        public int UnavailableHoursSummary { get; set; }
    }
}
