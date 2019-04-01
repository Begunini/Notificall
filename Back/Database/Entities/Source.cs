using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Source
    {
        [Key]
        public Guid Id { get; set; }

        //public Guid ClientId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Value { get; set; }

        public int? PhoneColumn { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        //public Client Client { get; set; }

        public virtual List<Text> Texts { get; set; }
    }
}
