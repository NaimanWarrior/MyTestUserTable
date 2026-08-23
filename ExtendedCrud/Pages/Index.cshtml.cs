using ExtendedCrud.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ExtendedCrud.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Введите Email")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Введите Email")]
            [EmailAddress(ErrorMessage = "Некорректный формат Email")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Введите пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public IActionResult OnGet(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Users/Index");
            }

            if(!string.IsNullOrWhiteSpace(returnUrl))
            {
                ErrorMessage = "You have to log in or register!";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //    return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ErrorMessage = "Неверный логин или пароль.";
                return Page();
            } else
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName!, Input.Password, false, false);
                if (result.Succeeded)
                {
                    user.LastLoginDate = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                    return RedirectToPage("/Users/Index");
                }
            }

            if (user.IsBlocked)
            {
                ErrorMessage = "Ваш аккаунт заблокирован.";
                return Page();
            }

            

            ErrorMessage = "Неверный логин или пароль.";
            return Page();
        }
    }
}
