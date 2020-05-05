using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class AboutUs
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "توضیحات")]
        public string Des { get; set; }

        [Display(Name = "عنوان اول")]
        public string Title1 { get; set; }

        [Display(Name = "عنوان دوم")]
        public string Title2 { get; set; }

        [Display(Name = "عنوان سوم")]
        public string Title3 { get; set; }

        [Display(Name = "عنوان چهارم")]
        public string Title4 { get; set; }

        [Display(Name = "عکس")]
        public string Image { get; set; }
    }
}
