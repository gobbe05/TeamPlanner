using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;

namespace TeamPlanner.Controllers
{
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
            if (worker == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Edit", "Group", groupId);
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
