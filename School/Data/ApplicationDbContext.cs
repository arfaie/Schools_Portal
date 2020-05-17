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
        public DbSet<Level> Levels { get; set; }




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
            builder.Entity<Level>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            addEducationType(builder);

            addLevel(builder);

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

        private static void addLevel(ModelBuilder builder)
        {
            builder.Entity<Level>().HasData(
                new Level {Id = "afde4ad493d2eb181cb87f9a", Title = "اول"},
                new Level {Id = "458813190d896a4dd601ae18", Title = "دوم"},
                new Level {Id = "7a7c4e6aade4c0a7636508ae", Title = "سوم"},
                new Level {Id = "239443dfa6a286607d777e71", Title = "چهارم"},
                new Level {Id = "1c76d0b4dde0e7a116273075", Title = "پنجم"},
                new Level {Id = "56b04a2c864e3706727f7d41", Title = "ششم"},
                new Level {Id = "72d54246a837967a52bfe5ad", Title = "هفتم"},
                new Level {Id = "738e40ca96f4308399678ab5", Title = "هشتم"},
                new Level {Id = "02e1122fc0d3a65db4cdc514", Title = "نهم"},
                new Level {Id = "cfd14d118be838a685e20e3e", Title = "دهم"},
                new Level {Id = "096a4bff94237b95c131ee9d", Title = "یازدهم"},
                new Level {Id = "4d414cba8d6965cc0a482381", Title = "دوازدهم"}
            );
        }
    }
}
