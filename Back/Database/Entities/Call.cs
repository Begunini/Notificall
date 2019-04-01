using System;
using System.ComponentModel.DataAnnotations;
using Database.Enums;

namespace Database.Entities
{
    public class Call
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required]
        public CallResult Result { get; set; }

        [Required]
        public string Phone { get; set; }

        public string MessageText { get; set; }

        //public Guid VoiceId { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public virtual Event Event { get; set; }

        //public virtual Voice Voice { get; set; }
    }
}
