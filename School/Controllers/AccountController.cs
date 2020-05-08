using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Models;
using School.Models.Helpers;
using School.Models.Helpers.OptionEnums;
using School.ViewModels;

namespace School.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(string statusMessage = "")
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (TempData["StatusMessage"] != null)
            {
                TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(login.userName);

                if (user?.IsBlocked == true)
                {
                    return View("AccessDenied");
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(login.userName, login.Password, false, true);
                if (result.Succeeded)
                {
                    var userRole = await _userManager.GetRolesAsync(user);

                    if (userRole.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "User", new { Area = "Admin" });
                    }

                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }

                ModelState.AddModelError(String.Empty, "نام کاربری یا رمز عبور نادرست است.");
                return View(login);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(String.Empty, "خطا در ورود کاربر.");
            return View(login);
        }
    }
}