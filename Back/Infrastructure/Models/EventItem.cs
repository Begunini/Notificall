using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class EventItem
    {
        public Guid EventId { get; set; }

        public Guid TextId { get; set; }

        //public Guid VoiceId { get; set; }

        public List<string> SourceHeaders{ get; set; }

        public List<string> SourceItem { get; set; }

        public string Phone { get; set; }
    }
}
