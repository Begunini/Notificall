using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        public virtual Billing Billing { get; set; }

        public virtual BillingPrice BillingPrice { get; set; }

        public virtual List<Combination> Combinations { get; set; }

        public virtual List<Source> Sources { get; set; }

        public virtual List<Voice> Voices { get; set; }

        public virtual List<Event> Events { get; set; }
     }
}
