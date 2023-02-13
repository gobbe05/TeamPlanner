using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly UserManager<Account> _userManager;

        public DashboardController(IAccountRepository accountRepository, IGroupRepository groupRepository, ITimeRepository timeRepository, UserManager<Account> userManager)
        {
            _accountRepository = accountRepository;
            _groupRepository = groupRepository;
            _timeRepository = timeRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Accepted == false) return RedirectToAction("Index", "UserNotFound");
            var times = await _timeRepository.GetAllTimesByWeekAsync("1", user.Id);
            var unavTimes = await _timeRepository.GetAllUnavailableTimesByWeekAsync("1", user.Id);
            var userSummary = await _accountRepository.GetUserSummaryByWeek("1", user.Id);

            var groups = await _groupRepository.GetAllGroupsByUser(user.Id);
            List<GroupSummary> groupSummaries = new List<GroupSummary>(); 

            foreach(var group in groups)
            {
                GroupSummary groupSummary = new GroupSummary
                {
                    Hours = group.TimeUsed,
                    Name = group.Name
                };
                groupSummaries.Add(groupSummary);
            }

            var VM = new DashboardViewModel
            {
                Week = 1,
                Times= times,
                UnavailableTimes= unavTimes,
                UserSummary = userSummary,
                Groups = groupSummaries
            };

            return View(VM);
        }
    }
}
