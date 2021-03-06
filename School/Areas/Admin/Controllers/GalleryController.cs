﻿using System;
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
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;

        public GalleryController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            ViewBag.Path = "/upload/thumbnailimage/";
            return View(await _context.Galleries.ToListAsync());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddEdit(string id)
        {
            var gallery = await _context.Galleries.FirstOrDefaultAsync(c => c.Id == id);
            if (gallery != null)
            {
                return PartialView("AddEdit", gallery);
            }

            return PartialView("AddEdit", new Gallery());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(string id, Gallery model, IEnumerable<IFormFile> files, string imgName)
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
                            model.Image = filename;
                        }
                        var image = new ImageResizer();
                        image.Resize(upload + filename, _env.WebRootPath + Helper.ThumbnailImagePath + filename);
                    }
                }

                if (String.IsNullOrWhiteSpace(id))
                {

                    if (model.Image == null)
                    {
                        model.Image = "defaultpic.png";
                    }
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["Notification"] = Notification.ShowNotif(MessageType.Add, ToastType.Green);
                    return Json(new { Status = "success" });
                }

                if (model.Image == null)
                {
                    model.Image = imgName;
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

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var gallery = await _context.Galleries.SingleOrDefaultAsync(a => a.Id == id);
            if (gallery == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("Delete", "تصویر گالری ");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string redirectUrl)
        {
            if (ModelState.IsValid)
            {
                {
                    var model = await _context.Galleries.SingleOrDefaultAsync(b => b.Id == id);

                    var sourcePath = Path.Combine(_env.WebRootPath.Replace("\\", "/") + Helper.NormalImagePath, model.Image);
                    if (System.IO.File.Exists(sourcePath))
                    {
                        System.IO.File.Delete(sourcePath);
                    }

                    var sourcePath2 = Path.Combine(_env.WebRootPath.Replace("\\", "/") + Helper.ThumbnailImagePath, model.Image);
                    if (System.IO.File.Exists(sourcePath2))
                    {
                        System.IO.File.Delete(sourcePath2);
                    }

                    _context.Remove(model);
                    await _context.SaveChangesAsync();
                }

                TempData["Notification"] = Notification.ShowNotif(MessageType.Delete, ToastType.Red);
                return PartialView("_SuccessfulResponse", redirectUrl);
            }

            TempData["Notification"] = Notification.ShowNotif(MessageType.DeleteError, ToastType.Red);
            return RedirectToAction("Index");
        }
    }
}