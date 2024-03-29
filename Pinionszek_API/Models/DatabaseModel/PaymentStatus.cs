﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Pinionszek_API.Models.DatabaseModel
{
    public class PaymentStatus
    {
        public int IdPaymentStatus { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Payment> Payment { get; set; }
    }
}
