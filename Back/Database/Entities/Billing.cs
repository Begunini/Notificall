using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Billing
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public decimal Balance { get; set; }

        public decimal BlockedAmount { get; set; }

        public virtual Client Client { get; set; }
    }
}
