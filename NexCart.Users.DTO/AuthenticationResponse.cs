namespace   NexCart.Users.DTO
{
    public class AuthenticationResponse
    {        public Guid UserId { get; set; }        public string? Email { get; set; }        public string? PersonName { get; set; }        public string? Gender { get; set; }        public string? Token { get; set; }        public bool Success { get; set; }        public AuthenticationResponse() { }        public AuthenticationResponse(Guid userId, string? email, string? personName, string? gender, string? token, bool success)        {            UserId = userId;            Email = email;            PersonName = personName;            Gender = gender;            Token = token;            Success = success;        }    }}
