using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Models;
using School.Models.Helpers;
using School.Models.Helpers.OptionEnums;

namespace School.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactUsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactUses.ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(string id)
        {
            var contactus = await _context.ContactUses.FirstOrDefaultAsync(c => c.Id == id);
            if (contactus != null)
            {
                return PartialView("AddEdit", contactus);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ContactUs model, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    return RedirectToAction("Index");
                }

                _context.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);

                return PartialView("_SuccessfulResponse", redirectUrl);
            }


            return PartialView("AddEdit", model);
        }

        //[HttpGet]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var contactUs = await _context.ContactUses.FirstOrDefaultAsync(c => c.Id == id);
        //    if (contactUs == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return PartialView("Delete", contactUs.Title);
        //}
    }
}