using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Email = registerViewModel.Email, UserName = registerViewModel.Email };
                var result = _userManager.CreateAsync(user, registerViewModel.Password);


                if (result.Result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }

            return View(registerViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result =
                    _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, false, false).Result;

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }

                return RedirectToAction("Index", "Item");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
