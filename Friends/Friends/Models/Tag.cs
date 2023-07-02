using System;
using System.Collections.Generic;
using System.Text;

namespace Friends.Models
{
    internal class Tag
    {
        public int Id { get; set; }
        public List<UserEvent> UserEvent { get; set; }
        public string Name { get; set; }    

    }
}
