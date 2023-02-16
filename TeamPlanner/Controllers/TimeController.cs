using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TeamPlanner.Data;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Controllers
{
    [Authorize]
    public class TimeController : Controller
    {
        private readonly ITimeRepository _timeRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly UserManager<Account> _userManager;
        private string _week;
        public TimeController(ITimeRepository timeRepository, UserManager<Account> userManager, IGroupRepository groupRepository)
        {
            _timeRepository = timeRepository;
            _userManager = userManager;
            _groupRepository = groupRepository;
            _week = ISOWeek.GetWeekOfYear(DateTime.Now).ToString();
        }
        public IActionResult Index()
        {
            return RedirectToAction("Overview");
        }
        public async Task<IActionResult> Overview()
        {
            var currentUser = _userManager.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            if(currentUser == null || currentUser.Accepted == false) return RedirectToAction("Index", "UserNotFound");
            var times = await _timeRepository.GetAllTimesByWeekAsync(_week, currentUser.Id);
            var unavailableTimes = await _timeRepository.GetAllUnavailableTimesByWeekAsync(_week, currentUser.Id);
            var viewModel = new OverviewViewModel
            {
                Week = _week,
                Times = times,
                UnavailableTimes = unavailableTimes,
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Overview(OverviewViewModel OverviewVM)
        {
            var currentUser = _userManager.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            var times = await _timeRepository.GetAllTimesByWeekAsync(OverviewVM.Week, currentUser.Id);
            var unavailableTimes = await _timeRepository.GetAllUnavailableTimesByWeekAsync(OverviewVM.Week, currentUser.Id);
            var viewModel = new OverviewViewModel
            {
                Times = times,
                UnavailableTimes = unavailableTimes,
                Week = OverviewVM.Week
            };
            return View(viewModel);
        }
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Admin() 
        {
            
            var currentUser = _userManager.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            var times = await _timeRepository.GetAllTimesByWeekAsync(_week, currentUser.Id);
            var unavailableTimes = await _timeRepository.GetAllUnavailableTimesByWeekAsync(_week, currentUser.Id);
            var viewModel = new AdminViewModel
            {
                Week = _week,
                Username = currentUser.UserName,
                Times = times,
                UnavailableTimes = unavailableTimes,
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Admin(AdminViewModel AdminVM)
        {
            var currentUser = _userManager.Users.FirstOrDefault(item => item.UserName == AdminVM.Username);
            if(currentUser == null)
            {
                TempData["Error"] = "User was not found";
                return View(AdminVM);
            }
            var times = await _timeRepository.GetAllTimesByWeekAsync(AdminVM.Week, currentUser.Id);
            var unavailableTimes = await _timeRepository.GetAllUnavailableTimesByWeekAsync(AdminVM.Week, currentUser.Id);

            var viewModel = new AdminViewModel
            {
                Times = times,
                UnavailableTimes = unavailableTimes,
                Week = AdminVM.Week,
                Username= AdminVM.Username,
            };
            return View(viewModel);
        }
        public IActionResult Create(string weekday, string week, string start, string username)
        {
            var groups = _groupRepository.GetAll();
            var vm = new CreateTimeViewModel
            {
                Groups = groups
            };
            return View(vm);
        }
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult AdminCreate(string weekday, string week, string start, string username)
        {
            var groups = _groupRepository.GetAll();
            var vm = new AdminCreateViewModel
            {
                Groups = groups
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateTime(CreateTimeViewModel TimeVM)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("AdminCreate", TimeVM);
            }
            var currentUser = _userManager.Users.FirstOrDefault(item => item.UserName == TimeVM.Username);
            var time = new Time
            {
                Start = TimeVM.Start,
                End = TimeVM.End,
                Week = TimeVM.Week,
                Weekday = TimeVM.Weekday,
                WorkerId = currentUser.Id,
                GroupId = TimeVM.GroupId,
                Salary = currentUser.Salary,
            };
            var result = _timeRepository.CreateTime(time);
            return RedirectToAction("Overview");
        }
        [HttpPost]
        public async Task<IActionResult> CreateUnavailableTime(CreateTimeViewModel TimeVM)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create", TimeVM);
            }
            var currentUser = _userManager.Users.FirstOrDefault(item => item.UserName == TimeVM.Username);
            var time = new UnavailableTime
            {
                Start = TimeVM.Start,
                End = TimeVM.End,
                Week = TimeVM.Week,
                Weekday = TimeVM.Weekday,
                WorkerId = currentUser.Id,
                GroupId = TimeVM.GroupId,
            };
            var result = _timeRepository.CreateUnavailableTime(time);
            if(result == false)
            {
                return RedirectToAction("Admin");
            }
            return RedirectToAction("Overview");
        }
    }
}
