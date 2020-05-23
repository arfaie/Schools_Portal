using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Cost
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string IdYear { get; set; }

        [ForeignKey("IdYear")]
        public virtual Year Year { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "شهریه")]
        public int Value { get; set; }

        [Display(Name = "توضیحات")]
        public string Des { get; set; }

        [Display(Name = "مقطع")]
        public string IdLevel { get; set; }

        [ForeignKey("IdLevel")]
        public virtual Level Level { get; set; }

    }
}
