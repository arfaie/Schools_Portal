using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class FactorItem
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "فاکتور")]
        public string FactorId { get; set; }

        [ForeignKey("FactorId")]
        public virtual Factor Factor { get; set; }

        [Display(Name = "هزینه")]
        public string CostId { get; set; }

        [ForeignKey("CostId")]
        public virtual Cost Cost { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }
    }
}
