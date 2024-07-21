
using CustomersApi.Models;
using CustomrsApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CustomersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest authData)
        {
            var response = await _userService.Authenticate(authData);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        // [HttpGet("Customers")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Customers()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        // [HttpPut("EditCustomer")]
        [HttpPut]
        [Authorize]
        [ValidateIsEditorAuthorized]
        public async Task<IActionResult> EditCustomer([FromBody] UpdateUserDTO userObj)
        {
            var result = await _userService.UpdateUser(userObj);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> CustomerByID(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _userService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}