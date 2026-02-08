using Microsoft.AspNetCore.Mvc;
using NexCart.Users.DTO;
using NexCart.Users.ServiceContracts;

namespace NexCart.UsersApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }
    [HttpGet("{userID}")]
    public async Task<IActionResult> GetUserByUserID(Guid userID)
    {
        if (userID == Guid.Empty)
        {
            return BadRequest(new ApiResponse<string>(false, "Invalid user ID", null));
        }
        UserDTO? user = await _usersService.GetUserByUserID(userID);
        if (user == null)
        {
            return NotFound(new ApiResponse<string>(false, "User not found", null));
        }
        return Ok(new ApiResponse<UserDTO?>(true, "User retrieved", user));
    }

}