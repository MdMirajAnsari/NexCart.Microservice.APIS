using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexCart.Users.DTO
{
    public class RegisterRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PersonName { get; set; }
        public GenderOptions Gender { get; set; }

        public RegisterRequest() { }

        public RegisterRequest(string? email, string? password, string? personName, GenderOptions gender)
        {
            Email = email;
            Password = password;
            PersonName = personName;
            Gender = gender;
        }
    }

}
