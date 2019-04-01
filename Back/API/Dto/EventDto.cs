using System;
using System.Collections.Generic;
using Database.Enums;

namespace API.Dto
{
    public class EventDto
    {

        public EventStatus Status { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        public string Source { get; set; }

        public string Text { get; set; }

        public List<CallDto> Calls { get; set; }
    }
}
