using Chat.AuthService.Infrastructure;
using Chat.AuthService.Web.Application.Messaging.ProfileMessages.ViewModels;
using Chat.AuthService.Web.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Chat.AuthService.Web.Pages.Profile
{
    [AllowAnonymous]
    public class RegisterModel(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager)
        : PageModel
    {
       // [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; } = null!;

        [BindProperty] public RegisterViewModel? Input { get; set; }
        public void OnGet() => Input = new RegisterViewModel
        {
         //   ReturnUrl = ReturnUrl
        };
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input != null)
            {
                var user = await userManager.FindByNameAsync(Input.Email); // email = username
                if (user != null)
                {
                    ModelState.AddModelError("Email", "User is found try another Useraname");
                    return Page();
                }

                CancellationToken cancellationToken = new CancellationToken();

                var usersss = new RegisterViewModel
                {
                    Email = Input.Email,
                    Password = Input.Password,
                    ConfirmPassword = Input.ConfirmPassword,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    ReturnUrl = "/success"
                };

                var reg = await accountService.RegisterAsync(usersss,cancellationToken);
                    

                 /*   if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }  */
                    return RedirectToPage("/swagger");
                
            }

            ModelState.AddModelError("Username", "Error");
            return Page();
        }
    }
}
