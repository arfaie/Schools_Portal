using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Factor
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "جمع مبلغ")]
        public int TotalPrice { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }

        [Display(Name = "کد فاکتور")]
        public string FactorCode { get; set; }

        [Display(Name = "پرداخت شده")]
        public bool IsPaid { get; set; }
        public ICollection<FactorItem> FactorItems { get; set; }


    }
}
