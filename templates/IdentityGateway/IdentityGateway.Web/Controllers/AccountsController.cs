using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityGateway.Web.Models.Accounts;

namespace IdentityGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    public class IdentityGatewayController : ControllerBase
    {
        public IdentityGatewayController(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public SignInManager<IdentityUser> SignInManager { get; }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LogIn(LoginRequest request)
        {
            var result = await SignInManager.PasswordSignInAsync(request.UserName, request.Password, true, false);
            if (result.Succeeded)
                return Ok();
            if (result.IsNotAllowed)
                return Forbid();
            if (result.RequiresTwoFactor)
                return BadRequest(new[] { "Two-factor authentication is required." });
            if (result.IsLockedOut)
                return BadRequest(new[] { "You're locked out." });
            return BadRequest(new[] { "Invalid credentials." });
        }

        [HttpPost("logout")]
        public Task LogOut()
        {
            return SignInManager.SignOutAsync();
        }
    }
}