using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;
using School.Helpers;
using School.Models;
using School.Models.Helpers;
using School.Models.Helpers.OptionEnums;

namespace School.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        public SettingController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index()
        {
            ViewBag.Path = "/upload/thumbnailimage/";
            return View(_context.Settings.FirstOrDefault());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            if (setting != null)
            {
                return PartialView("AddEdit", setting);
            }

            return PartialView("AddEdit", new Setting());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, Setting model, IEnumerable<IFormFile> files, string imgName)
        {
            if (ModelState.IsValid)
            {
                var upload = Path.Combine(_env.WebRootPath.Replace("\\", "/") + Helper.NormalImagePath);
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        await using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                            model.Logo = filename;
                        }
                        var image = new ImageResizer();
                        image.Resize(upload + filename, _env.WebRootPath + Helper.ThumbnailImagePath + filename);
                    }
                }

                if (String.IsNullOrWhiteSpace(id))
                {

                    if (model.Logo == null)
                    {
                        model.Logo = "defaultpic.png";
                    }
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
                    return Json(new { Status = "success" });
                }

                if (model.Logo == null)
                {
                    model.Logo = imgName;
                }

                _context.Update(model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = Notification.ShowNotif(MessageType.Edit, ToastType.Blue);
                return Json(new { Status = "success" });
            }

            var list = new List<string>();
            foreach (var validation in ViewData.ModelState.Values)
            {
                list.AddRange(validation.Errors.Select(error => error.ErrorMessage));
            }
            return Json(new { Status = "error" });
        }

    }
}