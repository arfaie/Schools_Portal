using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Setting
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "توضیحات گالری")]
        public string GalleryDes { get; set; }

        [Display(Name = "لگو")]
        public string Logo { get; set; }

        [Display(Name = "نام مدرسه")]
        public string SchoolName { get; private set; }
    }
}
