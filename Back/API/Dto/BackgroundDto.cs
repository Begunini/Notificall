using System;

namespace API.Dto
{
    public class BackgroundDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }
    }
}
