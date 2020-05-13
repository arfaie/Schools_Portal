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
        public DbSet<Models.Services> Serviceses { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<EducationType> EducationTypes { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<FactorItem> FactorItems { get; set; }
        public DbSet<Payment> Payments { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<AboutUs>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<ContactUs>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Gallery>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Models.Services>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Setting>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Slider>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Student>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<EducationType>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Year>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Cost>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Role>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Factor>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<FactorItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Payment>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            addEducationType(builder);

        }

        private static void addEducationType(ModelBuilder builder)
        {
            builder.Entity<EducationType>().HasData(
                new EducationType { Id = "6f9c65d681937c32dafcec01", Title = "زیر دیپلم" },
                new EducationType { Id = "6f9c65d681937c32dafcec03", Title = "دیپلم" },
                new EducationType { Id = "6f9c65d681937c32dafcec05", Title = "فوق دیپلم" },
                new EducationType { Id = "6f9c65d681937c32dafcec06", Title = "لیسانس" },
                new EducationType { Id = "6f9c65d681937c32dafcec07", Title = "فوق لیسانس" },
                new EducationType { Id = "6f9c65d681937c32dafcec08", Title = "دکتری" }
            );
        }
    }
}
