using CustomersApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using CustomrsApi.Filters;
using AuthorizeAttribute = CustomrsApi.Filters.AuthorizeAttribute;

namespace CustomersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IOutputCacheStore _cache;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IOutputCacheStore cache)
        {
            _userService = userService;
            _logger = logger;
            _cache = cache;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest authData)
        {
            var response = await _userService.Authenticate(authData);

            if (response == null)
                return BadRequest(new { message = "One of the given fields is incorrect." });

            return Ok(response);
        }

        [HttpGet]
        // [Authorize]
        [OutputCache(PolicyName = "UsersPolicy")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all users.");
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}.", id);
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                return NotFound();
            }
            return Ok(user);
        }


        [Authorize]
        [ValidateIsEditorAuthorized]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserDTO updatedUserObj, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating user with ID {UserId}.", updatedUserObj.Id);

            var updatedUser = await _userService.UpdateAsync(updatedUserObj);
            if (updatedUser != null)
            {
                await _cache.EvictByTagAsync("UsersPolicy_Tag", cancellationToken);
                return Ok(updatedUser); // Return the updated user
            }
            return NotFound(); // Return 404 if user was not found
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting user with ID {UserId}.", id);
            var deleted = await _userService.DeleteAsync(id);
            await _cache.EvictByTagAsync("UsersPolicy_Tag", cancellationToken);
            if (deleted)
            {
                return NoContent(); // User deleted successfully
            }
            else
            {
                return NotFound(); // User not found
            }
        }
    }
}
