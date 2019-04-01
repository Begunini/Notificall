using System;

namespace API.Dto
{
    public class CombinationDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid? VoiceId { get; set; }

        public Guid? TextId { get; set; }

        public Guid? BackgroundId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }
    }
}
