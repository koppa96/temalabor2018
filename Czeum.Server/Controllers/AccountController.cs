using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Czeum.Entities;
using Czeum.Server.Services;
using Czeum.Server.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Czeum.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ILogger<AccountController> logger) {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult> Login([FromBody]AppLoginModel model) {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByNameAsync(model.Username);

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded) {
                    _logger.LogInformation($"{model.Username} logged in.");

					var claims = new Claim[] {
						new Claim(ClaimTypes.Name, model.Username),
					};

					var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Connect4SecureSigningKey"));
                    var securityToken = new JwtSecurityToken(
                        issuer: "Czeum.Server",
                        audience: "Czeum.Server",
                        claims: claims,
						expires: DateTime.Now.AddHours(1),
                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(securityToken));
                }

	            return BadRequest("ErrorIncorrectLogin");
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [Route("/logout")]
		[Authorize]
        public async Task<ActionResult> Logout() {
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"{User.Identity.Name} logged out.");
            return Ok();
        }

        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult> Register([FromBody]AppRegisterModel model) {
            if (ModelState.IsValid) {
                if (await _userManager.FindByNameAsync(model.Username) != null) {
                    return BadRequest("ErrorUserExists");
                }

                if (model.Password != model.ConfirmPassword) {
                    return BadRequest("ErrorPasswordsNotMatching");
                }

                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation($"{model.Username} created new account");

					var claims = new Claim[] {
						new Claim(ClaimTypes.Name, model.Username)
					};

					var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Connect4SecureSigningKey"));
                    var securityToken = new JwtSecurityToken(
                        issuer: "Czeum.Server",
                        audience: "Czeum.Server",
                        expires: DateTime.UtcNow.AddHours(1),
						claims: claims,
                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(securityToken));
                }

                if (result.Errors.Count(e => e.Code == "DuplicateEmail") == 1) {
                    return BadRequest("ErrorDuplicateEmail");
                }

                if (result.Errors.Count(e =>
                        e.Code == "PasswordTooShort" || e.Code == "PasswordRequiresDigit" ||
                        e.Code == "PasswordRequiresUpper") > 0) {
                    return BadRequest("ErrorPasswordIncorrect");
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

		[HttpPost]
        [Route("/changePass")]
		[Authorize]
		public async Task<ActionResult> ChangePassword([FromBody]ChangePasswordModel model) {
			if (model.Password != model.ConfirmPassword) {
				return BadRequest("ErrorPasswordsNotMatching");
			}

			if (ModelState.IsValid) {
				ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
				var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

				if (result.Succeeded) {
					_logger.LogInformation($"{User.Identity.Name} changed their password.");
					return Ok("SuccessfulPasswordChange");
				}

				return BadRequest("OldPasswordIncorrect");
			}

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
    }
}