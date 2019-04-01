using System;
using Database.Enums;

namespace API.Dto
{
    public class TextDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Value { get; set; }

        public Language Language { get; set; }

        public Speaker Speaker { get; set; }

        public Emotion Emotion { get; set; }

        public decimal Speed { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }
    }
}
