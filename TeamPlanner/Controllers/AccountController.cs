using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TeamPlanner.Data;
using TeamPlanner.Interfaces;
using TeamPlanner.Models;
using TeamPlanner.ViewModels;

namespace TeamPlanner.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
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
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(); // Returns empty view
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel RegisterVM)
        {
            if(!ModelState.IsValid)
            {
                return View(RegisterVM); // Return view with the passed data
            }
            var emailUser = await _userManager.FindByEmailAsync(RegisterVM.Email); // Get user by email
            var usernameUser = await _userManager.FindByNameAsync(RegisterVM.Username); // Get user by username

            // User with the same email exists
            if (emailUser != null) 
            {
                TempData["Error"] = "This email address is already in use";
                return View(RegisterVM);
            }
            // User with the same username already exists 
            if (usernameUser !=  null) 
            {
                TempData["Error"] = "This username is alreay in use";
                return View(RegisterVM);
            }
            // Create user object with values
            var newUser = new Account 
            {
                FirstName = RegisterVM.FirstName,
                LastName = RegisterVM.LastName,
                Email = RegisterVM.Email,
                UserName = RegisterVM.Username,
                Accepted = false
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, RegisterVM.Password); // Create new user

            // Check if the user creation succeeded
            if(newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User); // Asign user role to the newly created user
            }
            else // Server side error
            {
                TempData["Error"] = "There was an error on our  side. Please try again";
                return View(RegisterVM);
            }
            return RedirectToAction("Login"); // Redirect to login page
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(); // Returns empty view
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel LoginVM)
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Modelstate is not valid";
                return View(LoginVM);
            }

            var user = await _userManager.FindByNameAsync(LoginVM.UserName); // Find user by username

            if(user != null)
            {
                // User is found
                var passwordCheck = await _userManager.CheckPasswordAsync(user, LoginVM.Password);
                if(passwordCheck)
                {
                    // Password correct
                    var result = await _signInManager.PasswordSignInAsync(user, LoginVM.Password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home"); // Redirect to home
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
        [AllowAnonymous]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync(); // Sign out
            return RedirectToAction("Index"); // Redirect to login page
        }
        public async Task<IActionResult> Index()
        {
            List<Account> accounts = await _accountRepository.GetAll(); // Get all accounts
            if(accounts == null)
            {
                return View("Error");
            }
            var model = new AcceptUserViewModel
            {
                Accounts = accounts
            };
            return View(model);
        }
        
        public async Task<IActionResult> Accept(string id)
        {
            var result = await _accountRepository.Accept(id);
            if (result != true) return View("Error");
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

            bool result = _accountRepository.Update(account);
            if(!result) return View("Error");
            
            return RedirectToAction("Index");
        }

    }
}
