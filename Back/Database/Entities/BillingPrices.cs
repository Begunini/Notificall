using System;
using System.ComponentModel.DataAnnotations;
using Database.Enums;

namespace Database.Entities
{
    public class BillingPrice
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public BillingType BillingType { get; set; }

        public decimal Cost { get; set; }

        public virtual Client Client { get; set; }
    }
}
