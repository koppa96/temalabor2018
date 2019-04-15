using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Czeum.DAL.Entities;
using Czeum.DTO;
using Czeum.DTO.UserManagement;
using Czeum.Server.Services.EmailSender;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Czeum.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("register")]
        public async Task<ActionResult> RegisterAsync([FromBody]RegisterModel model) {
	        if (!ModelState.IsValid)
	        {
		        return StatusCode(StatusCodes.Status500InternalServerError);
	        }

	        if (await _userManager.FindByNameAsync(model.Username) != null)
	        {
		        return BadRequest(ErrorCodes.UsernameAlreadyTaken);
	        }

	        if (model.Password != model.ConfirmPassword)
	        {
		        return BadRequest(ErrorCodes.PasswordsNotMatching);
	        }

	        var user = new ApplicationUser
	        {
				UserName = model.Username,
				Email = model.Email
	        };

	        var result = await _userManager.CreateAsync(user, model.Password);
	        if (result.Succeeded)
	        {
		        _logger.LogInformation($"New user created: {user.UserName}");
		        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.UserName));
		        await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Name, user.UserName));
		        await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Id, user.Id));

                string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailSender.SendConfirmationEmailAsync(user.Email, confirmationToken);

		        return Ok();
	        }

	        var errors = result.Errors.Select(e => e.Code);
	        return BadRequest(errors);
        }

		[HttpPost]
        [Route("change-password")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult> ChangePasswordAsync([FromBody]ChangePasswordModel model) {
			if (model.Password != model.ConfirmPassword) {
				return BadRequest(ErrorCodes.PasswordsNotMatching);
			}

			if (ModelState.IsValid) {
				ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
				var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

				if (result.Succeeded) {
					_logger.LogInformation($"{User.Identity.Name} changed their password.");
					return Ok();
				}

				return BadRequest(ErrorCodes.BadOldPassword);
			}

			return StatusCode(StatusCodes.Status500InternalServerError);
		}

        [HttpPost]
        [Route("confirm-email")]
        public async Task<ActionResult> ConfirmEmailAsync(string username, string token)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(username);
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return Ok();
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}