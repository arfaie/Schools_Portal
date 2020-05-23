using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;

namespace School.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FactorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FactorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Factors.Include(f => f.ApplicationUser).ToListAsync());
        }
    }
}