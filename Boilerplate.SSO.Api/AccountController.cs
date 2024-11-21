using Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.SSO.Api
{
    [Route("account")]
    [ApiController]
    public class AccountController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var result = await userManager.CreateAsync(request.ToApplicationUser(), request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
