using Chat.Web.TokenModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chat.Web.Pages
{
    public class LoginModel : PageModel
    {

        const string serverUrl = "https://localhost:10001/connect/token";


        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }
        [BindProperty] public LoginViewModel? Input { get; set; }

        public void OnGet() => Input = new LoginViewModel
        {
           
        };
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Input != null)
            {
                var token = await TokenLoader.RequestToken(Input.Username,Input.Password, serverUrl);

                if (string.IsNullOrWhiteSpace(token?.AccessToken))
                {
                    ModelState.AddModelError("Username", "Username or password incorrect");
                    return Page();

                }
                return Redirect($"Chat?token={token.AccessToken}&username={Input.Username}");
            }
            ModelState.AddModelError("Username", "Username or password incorrect");
            return Page();
        }
    }
}
