using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Combination
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public string Title { get; set; }

        public Guid? VoiceId { get; set; }

        public Guid? TextId { get; set; }

        public Guid? BackgroundId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        public virtual Client Client { get; set; }

        public virtual Voice Voice { get; set; }

        public virtual Text Text { get; set; }

        public virtual Background Background { get; set; }
    }
}
