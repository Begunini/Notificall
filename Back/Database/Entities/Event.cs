using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Database.Enums;

namespace Database.Entities
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }

        //public Guid ClientId { get; set; }

        //public decimal Price { get; set; }

        [Required]
        public Guid SourceId { get; set; }

        public Guid TextId { get; set; }

        //public Guid VoiceId { get; set; }

        [Required]
        public EventStatus Status { get; set; }

        //public Client Client { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        public virtual Source Source { get; set; }

        public virtual Text Text { get; set; }

        //public Voice Voice { get; set; }

        public virtual List<Call> Calls { get; set; }
    }
}
