using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Background
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        public virtual Client Client { get; set; }
    }
}
