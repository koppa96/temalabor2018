using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Czeum.Core.Services;
using Czeum.DAL;
using Czeum.Domain.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly CzeumContext context;
        private readonly IEmailService emailService;

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Kötelező")]
        [MinLength(6, ErrorMessage = "A jelszónak legalább 6 karakterből kell állnia")]
        [BindProperty]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string ConfirmPassword { get; set; } = "";

        [BindProperty]
        public string ReturnUrl { get; set; } = "";

        public RegisterModel(UserManager<User> userManager, CzeumContext context, IEmailService emailService)
        {
            this.userManager = userManager;
            this.context = context;
            this.emailService = emailService;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "login")
            {
                return Redirect(ReturnUrl);
            }

            await ValidateFieldsAsync();

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = Username,
                    Email = Email,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = await userManager.CreateAsync(user, Password);
                if (createResult.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Name, user.UserName));
                    await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Id, user.Id.ToString()));

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { username = user.UserName, token });

                    await emailService.SendConfirmationEmailAsync(user.Email, url);

                    return Redirect(ReturnUrl);
                }
                else
                {
                    AddModelErrorsForField(nameof(Username), createResult);
                    AddModelErrorsForField(nameof(Email), createResult);
                    AddModelErrorsForField(nameof(Password), createResult);
                }
            }

            return Page();
        }

        private void AddModelErrorsForField(string fieldName, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors.Where(x => x.Code.ToLower().Contains(fieldName.ToLower())))
            {
                ModelState.AddModelError(fieldName, error.Description);
            }
        }

        private async Task ValidateFieldsAsync()
        {
            var usernameTaken = Username != null && await context.Users.AnyAsync(x => x.UserName == Username);
            var emailTaken = Email != null && await context.Users.AnyAsync(x => x.Email == Email);

            if (usernameTaken)
            {
                ModelState.AddModelError(nameof(Username), "A felhasználónév már foglalt!");
            }

            if (emailTaken)
            {
                ModelState.AddModelError(nameof(Email), "Az e-mail cím már foglalt!");
            }

            if (ConfirmPassword != Password)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "A megadott jelszavak nem egyeznek!");
            }
        }
    }
}