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
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        if (registerRequest == null)
        {
            return BadRequest(new ApiResponse<string>(false, "Invalid registration data", null));
        }

        AuthenticationResponse? authenticationResponse = await _usersService.Register(registerRequest);

        if (authenticationResponse == null || authenticationResponse.Success == false)
        {
            return BadRequest(new ApiResponse<AuthenticationResponse?>(false, "Registration failed", authenticationResponse));
        }
        return Ok(new ApiResponse<AuthenticationResponse?>(true, "Registration successful", authenticationResponse));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (loginRequest == null)
        {
            return BadRequest(new ApiResponse<string>(false, "Invalid login data", null));
        }
        AuthenticationResponse? authenticationResponse = await _usersService.Login(loginRequest);
        if (authenticationResponse == null || authenticationResponse.Success == false)
        {
            return BadRequest(new ApiResponse<AuthenticationResponse?>(false, "Login failed", authenticationResponse));
        }
        return Ok(new ApiResponse<AuthenticationResponse?>(true, "Login successful", authenticationResponse));
    }
}