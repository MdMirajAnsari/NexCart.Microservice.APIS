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
            return new AuthenticationResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                PersonName = user.PersonName,
                Gender = user.Gender,
                Token = "token",
            };

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
            return new AuthenticationResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                PersonName = user.PersonName,
                Gender = user.Gender,
                Token = "token",
            };
        }
    }

    public async Task<UserDTO> GetUserByUserID(Guid userId)
    {
        ApplicationUser? user = await _usersRepository.GetUserByUserId(userId);

        if (user == null)
            return null;

        return new UserDTO
        {
            UserId = user.UserId,
            Email = user.Email,
            PersonName = user.PersonName,
            Gender = user.Gender ?? string.Empty
        };
    }
}