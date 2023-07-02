using System;
using System.Collections.Generic;
using System.Text;

namespace Friends.Models
{
    internal class Response
    {
        public int Id { get; set; }
        public int UserEventId { get; set; }
        public UserEvent UserEvent { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Type { get; set; }
    }
}
