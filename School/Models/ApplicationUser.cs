using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "کد ملی")]
        public long NationalCode { get; set; }

        [Display(Name = "زمان ثبت نام")]
        public DateTime RegistrationDateAndTime { get; set; }

        [Display(Name = "مسدود")]
        public bool IsBlocked { get; set; }

        [NotMapped]
        [Display(Name = "نقش")]
        public string ApplicationRoleId { get; set; }

        [NotMapped]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}