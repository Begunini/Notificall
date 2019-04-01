using System;

namespace API.Dto
{
    public class VoiceDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }
    }
}
