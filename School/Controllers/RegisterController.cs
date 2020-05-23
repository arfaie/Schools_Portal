using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _env;

        public RegisterController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ISmsSender smsSender, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _context = context;
            _smsSender = smsSender;
            _env = env;
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
        public async Task<IActionResult> RegisterStudent(Student model, IFormFile files, IFormFile files2)
        {
            if (ModelState.IsValid)
            {

                var upload = Path.Combine(_env.WebRootPath.Replace("\\", "/") + Helper.NormalImagePath);
               
                    if (files != null && files.Length > 0)
                    {
                        var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(files.FileName);
                        await using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                        {
                            await files.CopyToAsync(fs);
                            model.reportImage = filename;
                        }
                        var image = new ImageResizer();
                        image.Resize(upload + filename, _env.WebRootPath + Helper.ThumbnailImagePath + filename);
                    }

                    if (files2 != null && files2.Length > 0)
                    {
                        var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(files2.FileName);
                        await using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                        {
                            await files2.CopyToAsync(fs);
                            model.nationalCodeImage = filename;
                        }
                        var image = new ImageResizer();
                        image.Resize(upload + filename, _env.WebRootPath + Helper.ThumbnailImagePath + filename);
                    }


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

            return View("Index");
        }

    }
}
