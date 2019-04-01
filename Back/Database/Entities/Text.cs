using System;
using System.ComponentModel.DataAnnotations;
using Database.Enums;

namespace Database.Entities
{
    public class Text
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SourceId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public Language Language { get; set; }

        [Required]
        public Speaker Speaker { get; set; }

        [Required]
        public Emotion Emotion { get; set; }

        [Required]
        public decimal Speed { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        public virtual Source Source { get; set; }
    }
}
