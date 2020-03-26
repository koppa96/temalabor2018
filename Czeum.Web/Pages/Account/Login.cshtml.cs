using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Domain.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Czeum.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IUserClaimsPrincipalFactory<User> claimsPrincipalFactory;
        private readonly UserManager<User> userManager;

        [Required]
        [BindProperty]
        public string Username { get; set; } = "";

        [Required]
        [BindProperty]
        public string Password { get; set; } = "";

        [BindProperty]
        public bool RememberMe { get; set; } = false;

        [BindProperty]
        public string ReturnUrl { get; set; } = "";

        public LoginModel(
            IIdentityServerInteractionService interactionService,
            IUserClaimsPrincipalFactory<User> claimsPrincipalFactory,
            UserManager<User> userManager)
        {
            this.interactionService = interactionService;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
            this.userManager = userManager;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Username);
                if (user != null && await userManager.CheckPasswordAsync(user, Password))
                {
                    var signInProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                        AllowRefresh = true,
                        RedirectUri = ReturnUrl,
                        IsPersistent = RememberMe
                    };

                    var claimsPrincipal = await claimsPrincipalFactory.CreateAsync(user);
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal, signInProperties);
                    HttpContext.User = claimsPrincipal;

                    if (interactionService.IsValidReturnUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                }
            }

            return Page();
        }
    }
}