using Microsoft.AspNetCore.Mvc;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IAccountRepository _accountRepository;
        public GroupController(IGroupRepository groupRepository, IAccountRepository accountRepository)
        {
            _groupRepository = groupRepository;
            _accountRepository = accountRepository;
        }
        public IActionResult Index()
        {
            var groups = _groupRepository.GetAll();
            return View(groups);
        }
        public async Task<IActionResult> Details(int id) 
        { 
            var group = _groupRepository.GetById(id);
            var users = await _groupRepository.GetAllUsersByGroup(id);
            var model = new GroupDetailsViewModel
            {
                Group = group,
                Accounts = users
            };
            if (group == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Details(int id, string week)
        {
            var group = _groupRepository.GetById(id);
            List<AccountSummary> users;
            if(week == null)
            {
                users = await _groupRepository.GetAllUsersByGroup(id);
            }
            else
            {
                users = await _groupRepository.GetAllUsersByGroup(id, week);
            }
            var model = new GroupDetailsViewModel
            {
                Group = group,
                Accounts = users,
                Week = week
            };
            if (group == null)
            {
                RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Group groupVM)
        {
            if(!ModelState.IsValid)
            {
                return View(groupVM);
            }
            var check = _groupRepository.GetByName(groupVM.Name);
            if (check != null)
            {
                TempData["Error"] = "Item with that name already exists";
                return View(groupVM);
            }
            _groupRepository.Create(groupVM);
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var group = _groupRepository.GetById(id);
            var users = await _groupRepository.GetAllUsersByGroup(id);
            var EditVM = new EditGroupViewModel
            {
                group = group,
                Users = users
            };
            return View(EditVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditGroupViewModel groupVM) 
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "There was an error with the data";
                return View(groupVM);
            }
            var result = _groupRepository.Update(groupVM.group);
            if(result != true)
            {
                TempData["Error"] = "There was an error with the data";
                return View(groupVM);
            }
            return View(groupVM);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var group = _groupRepository.GetById(id);
            _groupRepository.Delete(group);
            return RedirectToAction("Index");
        }

    }
}
