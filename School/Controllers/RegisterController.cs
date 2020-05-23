using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Helpers;
using School.Helpers.OptionEnums;
using School.Models;
using School.Services;

namespace School.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ISmsSender _smsSender;

        public RegisterController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ISmsSender smsSender)
        {
            _userManager = userManager;
            _context = context;
            _smsSender = smsSender;
        }

        public async Task<IActionResult> Index()
        {
            var User = await _userManager.GetUserAsync(HttpContext.User);

            if (User == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (HttpContext.Session.GetInt32("acceptedrole") == null)
            {
                return RedirectToAction("Roles", "Home");
            }
            

            string SchoolName = _context.Settings.FirstOrDefault().SchoolName;
            ViewBag.SchoolName = SchoolName;
            var select = await _context.Students.FirstOrDefaultAsync(s => s.IdUser == User.Id);

            ViewBag.Level = new SelectList(await _context.Levels.ToListAsync(), "Id", "Title");

            ViewBag.educationType = new SelectList(await _context.EducationTypes.ToListAsync(), "Id", "Title");

            return View(select);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(Student model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var Mobile = user.PhoneNumber;
                string Code = Helper.GenerateShortenCode(Mobile, null).ToString();
                model.IdUser = user.Id;
                model.RegisterCode = int.Parse(Code);
                if (model.Id != null)
                {
                    _context.Students.Update(model);
                }
                else
                {
                    _context.Students.Add(model);

                    var SchoolName = _context.Settings.FirstOrDefault().SchoolName;

                    var fullName = model.FirstName + " " + model.LastName;

                    await _smsSender.SendSmsAsync(Mobile, SmsTypes.SchoolRegisterDone,
                        Code, SchoolName, fullName);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Level = new SelectList(await _context.Levels.ToListAsync(), "Id", "Title");

            ViewBag.educationType = new SelectList(await _context.EducationTypes.ToListAsync(), "Id", "Title");

            return View("Index", model);
        }
    }
}