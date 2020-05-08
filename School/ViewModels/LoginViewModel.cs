using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace School.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "نام کاربری")]
        [Remote("VerifyMobile", "Account")]
        public string userName { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
    }
}
