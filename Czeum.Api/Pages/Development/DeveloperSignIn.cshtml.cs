using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace Czeum.Api.Pages.Development
{
    public class DeveloperSignInModel : PageModel
    {
        [BindProperty]
        public string? Username { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? AccessToken { get; set; }

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            var formData = new Dictionary<string, string?>
            {
                { "client_id", "SwaggerClient" },
                { "client_secret", "SwaggerClientSecret" },
                { "grant_type", "password" },
                { "username", Username },
                { "password", Password },
                { "scope", "czeum_api" }
            };
            

            var content = new FormUrlEncodedContent(formData);
            using var client = new HttpClient();

            var response = await client.PostAsync("https://localhost:5001/connect/token", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(json);
                AccessToken = jObject.GetValue("access_token").Value<string>();
            }
        }
    }
}