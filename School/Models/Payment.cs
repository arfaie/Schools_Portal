using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Payment
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "فاکتور")]
        public string FactorId { get; set; }

        [ForeignKey("FactorId")]
        public virtual Factor Factor { get; set; }

        [Display(Name = "شماره تراکنش")]
        public string TransactionNumber { get; set; }

        [Display(Name = "وضعیت تراکنش")]
        public bool TransactionStatus { get; set; }

        [Display(Name = "تاریخ تراکنش")]
        public DateTime TransactionDate { get; set; }
    }
}
