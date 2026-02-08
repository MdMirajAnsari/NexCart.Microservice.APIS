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

        public UserDTO() { }

        public UserDTO(Guid userId, string? email, string? personName, string gender)
        {
            UserId = userId;
            Email = email;
            PersonName = personName;
            Gender = gender;
        }
    }

}
