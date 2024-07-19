using System.ComponentModel.DataAnnotations;

namespace Chat.Web.Pages
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; } = null!;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
    }
}
