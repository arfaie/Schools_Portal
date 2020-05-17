﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var select = await _context.Students.FirstOrDefaultAsync(s => s.IdUser == User.Id);

            //if (select.IsPreSubmit)
            //{

            //}

            ViewBag.educationType = new SelectList(await _context.EducationTypes.ToListAsync(), "Id", "Title");

            return View();
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

                model.IdUser = user.Id;

                _context.Students.Add(model);
                await _context.SaveChangesAsync();


                var Mobile = user.PhoneNumber;

                var SchoolName = _context.Settings.FirstOrDefault().SchoolName;

                var link = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                await _smsSender.SendSmsAsync(Mobile, SmsTypes.SchoolRegisterDone,
                    Helper.GenerateShortenCode(Mobile, link).ToString(), SchoolName);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.educationType = new SelectList(await _context.EducationTypes.ToListAsync(), "Id", "Title");

            return View("Index", model);
        }
    }
}