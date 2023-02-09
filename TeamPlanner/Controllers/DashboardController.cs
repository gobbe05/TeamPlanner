using Microsoft.AspNetCore.Mvc;
using TeamPlanner.Interfaces;

namespace TeamPlanner.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ITimeRepository _timeRepository;

        public DashboardController(IAccountRepository accountRepository, IGroupRepository groupRepository, ITimeRepository timeRepository)
        {
            _accountRepository = accountRepository;
            _groupRepository = groupRepository;
            _timeRepository = timeRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
