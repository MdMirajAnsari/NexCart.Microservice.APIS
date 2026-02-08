using System.ComponentModel.DataAnnotations;

namespace   NexCart.Users.DTO
{
    public class AuthenticationResponse
    {        [Key]        public Guid UserId { get; set; }        public string? Email { get; set; }        public string? PersonName { get; set; }        public string? Gender { get; set; }        public string? Token { get; set; }    }}
