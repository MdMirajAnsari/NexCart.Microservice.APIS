using Microsoft.AspNetCore.Mvc;
using NexCart.Users.DTO;
using NexCart.Users.ServiceContracts;

namespace NexCart.UsersApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUsersService _usersService;

    public AuthController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid registration data",
                Data = null
            });
        }

        AuthenticationResponse? authenticationResponse =
            await _usersService.Register(registerRequest);

        if (authenticationResponse == null)
        {
            return BadRequest(new ApiResponse<AuthenticationResponse?>
            {
                Success = false,
                Message = "Registration failed",
                Data = authenticationResponse
            });
        }

        return Ok(new ApiResponse<AuthenticationResponse?>
        {
            Success = true,
            Message = "Registration successful",
            Data = authenticationResponse
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid login data",
                Data = null
            });
        }

        AuthenticationResponse? authenticationResponse =
            await _usersService.Login(loginRequest);

        if (authenticationResponse == null)
        {
            return Unauthorized(new ApiResponse<AuthenticationResponse?>
            {
                Success = false,
                Message = "Invalid email or password",
                Data = null
            });
        }

        return Ok(new ApiResponse<AuthenticationResponse?>
        {
            Success = true,
            Message = "Login successful",
            Data = authenticationResponse
        });
    }


}