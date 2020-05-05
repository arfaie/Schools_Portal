using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Helpers;
using School.Models;
using School.Models.Helpers;
using School.Models.Helpers.OptionEnums;

namespace School.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add()
        {
            ViewBag.ApplicationRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");

            return PartialView("Add", new ApplicationUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ApplicationUser model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                model.RegistrationDateAndTime = DateTime.UtcNow;
                model.UserName = model.UserName;
                model.Email = model.Email;
                model.PhoneNumber = model.PhoneNumber;
                model.EmailConfirmed = true;
                model.PhoneNumberConfirmed = true;

                var result = await _userManager.CreateAsync(model, model.Password);
                if (result.Succeeded)
                {
                    var applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                    if (applicationRole != null)
                    {
                        var newRole = await _userManager.AddToRoleAsync(model, applicationRole.Name);
                        if (!newRole.Succeeded)
                        {
                            TempData["Notification"] = Notification.ShowNotif("خطا در تعیین نقش کاربر", ToastType.Red);

                            return PartialView("_SuccessfulResponse", redirectUrl);
                        }
                    }

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);

                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                TempData["Notification"] = Notification.ShowNotif("خطا در ثبت کاربر جدید", ToastType.Red);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            ViewBag.ApplicationRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");

            return PartialView("Add", model);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                    if (!String.IsNullOrWhiteSpace(userRole))
                    {
                        var role = await _roleManager.FindByNameAsync(userRole);

                        if (role != null)
                        {
                            user.ApplicationRoleId = role.Id;
                        }
                    }

                    ViewBag.ApplicationRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", user.ApplicationRoleId);

                    return PartialView("Edit", user);
                }
            }

            return PartialView("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    TempData["Notification"] = Notification.ShowNotif("خطا در یافتن کاربر", ToastType.Red);

                    return PartialView("_SuccessfulResponse", redirectUrl);
                }

                user.FullName = model.FullName;
                user.NationalCode = model.NationalCode;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.ApplicationRoleId = model.ApplicationRoleId;
                user.PhoneNumberConfirmed = true;
                user.EmailConfirmed = true;


                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                    if (applicationRole != null)
                    {
                        var allRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, allRoles);

                        var newRole = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (!newRole.Succeeded)
                        {
                            TempData["Notification"] = Notification.ShowNotif("خطا در تعیین نقش کاربر", ToastType.Red);

                            return PartialView("_SuccessfulResponse", redirectUrl);
                        }
                    }

                    TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                    return PartialView("_SuccessfulResponse", redirectUrl);
                }
            }

            var userRole = (await _userManager.GetRolesAsync(model)).FirstOrDefault();

            if (!String.IsNullOrWhiteSpace(userRole))
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    model.ApplicationRoleId = role.Id;
                }
            }

            ViewBag.ApplicationRoles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.ApplicationRoleId);

            return PartialView("Edit", model);
        }
    }
}