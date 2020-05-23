﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public class Order
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "فاکتور")]
        public string FactorId { get; set; }

        [Display(Name = "فاکتور")]
        public virtual Factor Factor { get; set; }

        [Display(Name = "شماره تراکنش")]
        public string TransactionNumber { get; set; }

        [Display(Name = "وضعیت تراکنش")]
        public bool TransactionStatus { get; set; }

        [Display(Name = "تاریخ تراکنش")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusId { get; set; }

        [Display(Name = "وضعیت")]
        public virtual Status Status { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}
