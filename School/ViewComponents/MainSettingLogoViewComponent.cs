using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Data;

namespace School.ViewComponents
{
    public class MainSettingLogoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MainSettingLogoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Settings.AsNoTracking().FirstOrDefaultAsync());
        }
    }
}
