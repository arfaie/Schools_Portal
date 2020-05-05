using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Models;

namespace School.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<AboutUs> AboutUses { get; set; }
        public DbSet<ContactUs> ContactUses { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Services> Serviceses { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<AboutUs>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<ContactUs>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Gallery>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Services>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Setting>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Slider>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

        }
    }
}
