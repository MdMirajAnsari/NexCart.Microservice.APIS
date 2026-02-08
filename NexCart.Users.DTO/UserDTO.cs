using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexCart.Users.DTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? PersonName { get; set; }
        public string Gender { get; set; }

    }

}
