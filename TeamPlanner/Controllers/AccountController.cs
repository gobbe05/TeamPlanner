using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamPlanner.Data;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly IAccountRepository _accountRepository;
        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager, IAccountRepository accountRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountRepository = accountRepository;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel RegisterVM)
        {
            if(!ModelState.IsValid)
            {
                return View(RegisterVM);
            }
            var emailUser = await _userManager.FindByEmailAsync(RegisterVM.Email);
            var usernameUser = await _userManager.FindByNameAsync(RegisterVM.Username);
            if(emailUser != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(RegisterVM);
            }
            if(usernameUser !=  null)
            {
                TempData["Error"] = "This username is alreay in use";
            }
            var newUser = new Account
            {
                FirstName = RegisterVM.FirstName,
                LastName = RegisterVM.LastName,
                Email = RegisterVM.Email,
                UserName = RegisterVM.Username,
                Accepted = false
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, RegisterVM.Password);

            if(newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            else
            {
                TempData["Error"] = "There was an error on our  side. Please try again";
                return View(RegisterVM);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel LoginVM)
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Modelstate is not valid";
                return View(LoginVM);
            }

            var user = await _userManager.FindByNameAsync(LoginVM.UserName);

            if(user != null)
            {
                //User is found
                var passwordCheck = await _userManager.CheckPasswordAsync(user, LoginVM.Password);
                if(passwordCheck)
                {
                    //Password correct
                    var result = await _signInManager.PasswordSignInAsync(user, LoginVM.Password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["Error"] = "Wrong credentials. Please, try again";
                    return View(LoginVM);
                }
            }
            else
            {
                TempData["Error"] = "User is not found";
                return View(LoginVM);
            }
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var model = new AcceptUserViewModel
            {
                Accounts = await _accountRepository.GetAll()
            };
            return View(model);
        }
        public async Task<IActionResult> Accept(string id)
        {
            await _accountRepository.Accept(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _accountRepository.GetById(id);
            if(user == null)
            {
                return RedirectToAction("Index");
            }
            var AccountVM = new AccountEditViewModel
            {
                Work = user.Work,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Salary = user.Salary,
                Phone = user.Phone,
                Address = user.Address,
            };
            return View(AccountVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, AccountEditViewModel accountVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", accountVM);
            }
            var account = await _accountRepository.GetByIdAsyncNoTracking(id);
            if(account == null)
            {
                return View("Error");
            }

            account.Id = id;
            account.Work = accountVM.Work;
            account.FirstName = accountVM.FirstName;
            account.LastName = accountVM.LastName;
            account.Salary = accountVM.Salary;
            account.Phone = accountVM.Phone;
            account.Address = accountVM.Address;

            _accountRepository.Update(account);
            
            return RedirectToAction("Index");
        }

    }
}
