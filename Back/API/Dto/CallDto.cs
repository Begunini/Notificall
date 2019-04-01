using System;
using Database.Enums;

namespace API.Dto
{
    public class CallDto
    {
        public CallResult Result { get; set; }

        public string Phone { get; set; }

        public string MessageText { get; set; }

        public DateTime Created { get; set; }
    }
}
