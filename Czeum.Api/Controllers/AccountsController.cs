using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Czeum.Api.Common;
using Czeum.Application.Services.EmailSender;
using Czeum.Domain.Entities;
using Czeum.DTO;
using Czeum.DTO.UserManagement;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Czeum.Api.Controllers
{
    [Route(ApiResources.Accounts.BasePath)]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;

        public AccountsController(UserManager<User> userManager, ILogger<AccountsController> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("register")]
        public async Task<ActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
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

	        var user = new User
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
		        await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Id, user.Id.ToString()));

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", id = user.Id, code = token },
                    protocol: Request.Scheme);

                await _emailService.SendConfirmationEmailAsync(user.Email, user.Id, token, url);

		        return Ok();
	        }

	        var errors = result.Errors.Select(e => e.Code);
	        return BadRequest(errors);
        }

		[HttpPost]
        [Route("change-password")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[Authorize]
		public async Task<ActionResult> ChangePasswordAsync([FromBody]ChangePasswordModel model)
        {
			if (model.Password != model.ConfirmPassword) {
				return BadRequest(ErrorCodes.PasswordsNotMatching);
			}

			if (ModelState.IsValid) {
				User user = await _userManager.FindByNameAsync(User.Identity.Name);
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("confirm-email")]
        public async Task<ActionResult> ConfirmEmailAsync(string username, string token)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return NotFound(ErrorCodes.NoSuchUser);
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"{username} has confirmed their email.");
                    return Ok();
                }

                return BadRequest();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        [Route("reset-password")]
        public async Task<ActionResult> GetPasswordResetTokenAsync(string username, string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(ErrorCodes.NoSuchUser);
                }
                if (user.UserName != username)
                {
                    return BadRequest(ErrorCodes.NoSuchUser);
                }

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var url = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", resetToken },
                    protocol: Request.Scheme);

                await _emailService.SendPasswordResetEmailAsync(email, resetToken, url);
                _logger.LogInformation($"Password reset email for {username} was sent to {email}.");

                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<ActionResult> ResetPasswordAsync([FromBody]PasswordResetModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest(ErrorCodes.PasswordsNotMatching);
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"{user.UserName} has successfully reset their password.");
                    return Ok();
                }

                var errors = result.Errors.Select(e => e.Code);
                return BadRequest(errors);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        [Route("resend-confirm-email")]
        public async Task<ActionResult> ResendConfirmationEmailAsync(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(ErrorCodes.NoSuchUser);
                }
                if (user.EmailConfirmed)
                {
                    return BadRequest();
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", id = user.Id, code = token },
                    protocol: Request.Scheme);

                await _emailService.SendConfirmationEmailAsync(email, user.Id, token, url);
                _logger.LogInformation($"Confirmation email resent to {email}.");

                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}