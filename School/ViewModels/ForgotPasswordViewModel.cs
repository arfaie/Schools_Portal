using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace School.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Phone]
        [Display(Name = "شماره موبایل")]
        public string Mobile { get; set; }

        public bool IsResetPassword { get; set; }
    }
}
