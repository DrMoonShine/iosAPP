using System;
using System.Collections.Generic;
using System.Text;

namespace Friends.Models
{
    [Serializable]
    public class UserDbo
    {
        
        public string NickName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public string VkLink { get; set; }
        public string TgLink { get; set; }
        public string Discription { get; set; }

    }
}
