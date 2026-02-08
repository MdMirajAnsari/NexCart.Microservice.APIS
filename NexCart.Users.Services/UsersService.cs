using NexCart.Users.DTO;

using NexCart.Users.Entities;
using NexCart.Users.RepositoryContracts;
using NexCart.Users.ServiceContracts;

namespace NexCart.Users.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        ApplicationUser? user =
            await _usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

        if (user == null)
        {
            return null;
        }
        else
        {
            return new AuthenticationResponse(user.UserId, user.Email, user.PersonName, user.Gender, "token", true);

        }
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        ApplicationUser user = new ApplicationUser()
        {
            PersonName = registerRequest.PersonName,
            Email = registerRequest.Email,
            Password = registerRequest.Password,
            Gender = registerRequest.Gender.ToString()
        };
        ApplicationUser? registeredUser = await _usersRepository.AddUser(user);

        if (registeredUser == null)
        {
            return null;
        }
        else
        {
            return new AuthenticationResponse(registeredUser.UserId, registeredUser.Email, registeredUser.PersonName, registeredUser.Gender, "token", true);
        }
    }

    public async Task<UserDTO> GetUserByUserID(Guid userId)
    {
        ApplicationUser user = await _usersRepository.GetUserByUserId(userId);
        return new UserDTO(user.UserId, user.Email, user.PersonName, user.Gender ?? string.Empty);
    }
}