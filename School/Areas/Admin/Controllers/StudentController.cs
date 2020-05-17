using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models.Helpers;
using School.Models.Helpers.OptionEnums;

namespace School.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.Include(s => s.EducationTypeFather).Include(s => s.EducationTypeMother).ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Accept(string id)
        {
            var student = await _context.Students.Include(s => s.EducationTypeFather).Include(s => s.EducationTypeMother).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Accept", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(string id,string redirectUrl)
        {
            var select = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

            _context.Students.Update(select);
            await _context.SaveChangesAsync();

            TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
            return PartialView("_SuccessfulResponse", "/Admin/Student");
        }

        
    }
}