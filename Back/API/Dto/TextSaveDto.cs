using System;
using Database.Enums;

namespace API.Dto
{
    public class TextSaveDto
    {
        public Guid SourceId { get; set; }

        public string Title { get; set; }

        public string Value { get; set; }

        public Language Language { get; set; }

        public Speaker Speaker { get; set; }

        public Emotion Emotion { get; set; }

        public decimal Speed { get; set; }
    }
}
