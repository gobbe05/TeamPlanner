using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamPlanner.Data;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class GroupUserController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        private readonly UserManager<Account> _userManager;
        public GroupUserController(IGroupRepository groupRepository, UserManager<Account> userManager)
        {
            _groupRepository = groupRepository;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Create(int groupId, string username)
        {
            var worker = await _userManager.FindByNameAsync(username);
            var group = _groupRepository.GetById(groupId);
            if (worker == null)
            {
                TempData["Error"] = "User not found";
                var vm = new EditGroupViewModel
                {
                    group = group
                };

                return RedirectToAction("Edit", "Group", new {Id = groupId});
            }
            var groupUser = new GroupUser
            {
                GroupId = groupId,
                WorkerId = worker.Id
            };
            _groupRepository.CreateLink(groupUser);
            return RedirectToAction("Index", "Group");
        }
    }
}
