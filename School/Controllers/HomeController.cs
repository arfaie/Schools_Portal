using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using School.Data;
using School.Models;

namespace School.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            string SchoolName = _context.Settings.FirstOrDefault().SchoolName;
            ViewBag.SchoolName = SchoolName;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Roles()
        {
            ViewBag.checkbox = "text-info";
            var select = _context.Roles;

            ViewBag.checkbox = "";

            return View(select);
        }

        [HttpPost]
        public IActionResult Roles(int acceptedrole)
        {
            if (acceptedrole == 1)
            {
                HttpContext.Session.SetInt32("acceptedrole",1);
                ViewBag.checkbox = "";

                return RedirectToAction("Index", "Register");
            }

            ViewBag.checkbox = "text-danger";

            var select = _context.Roles;

            return View(select);
        }
    }
}
