using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Czeum.Web.Pages.Account
{
    public class GetPasswordResetModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [Required(ErrorMessage = "Kötelező")]
        [EmailAddress(ErrorMessage = "Nem érvényes e-mail cím")]
        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string? ReturnUrl { get; set; } = "";

        public bool SendSuccessful { get; set; }

        public GetPasswordResetModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet(string? returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "cancel")
            {
                return Redirect(ReturnUrl);
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(Email), "Nem létezik ilyen e-mail című felhasználó");
                }
                else
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    // TODO: Send e-mail

                    SendSuccessful = true;
                }
            }

            return Page();
        }
    }
}