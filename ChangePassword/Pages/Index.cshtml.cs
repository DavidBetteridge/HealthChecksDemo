using ChangePassword.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ChangePassword.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PwnedPasswords _pwnedPasswords;
        private readonly DBContext _context;

        [BindProperty]
        public string? Password { get; set; }

        [BindProperty]
        public string? ConfirmPassword { get; set; }

        [BindProperty]
        public string? Message { get; set; }

        public IndexModel(PwnedPasswords pwnedPasswords, DBContext context)
        {
            _pwnedPasswords = pwnedPasswords;
            _context = context;
        }

        public async Task<ActionResult> OnPost()
        {

            if (!Password.Equals(ConfirmPassword))
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "The passwords must match");
                return Page();
            }

            if (await _pwnedPasswords.IsPasswordBanned(Password))
            {
                ModelState.AddModelError(nameof(Password), "This password is too common, please use a different password. ");
                return Page();
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == "david");
            if (user is null) throw new Exception("Your user account is missing from the database!");
            user.PasswordHash = HashPassword(Password);
            await _context.SaveChangesAsync();


            Password = string.Empty;
            ConfirmPassword = string.Empty;
            Message = "Your password was successfully changed";

            return Page();
        }

        private string HashPassword(string plainTextPassword)
        {
            var costParameter = 12;
            return BCrypt.Net.BCrypt.EnhancedHashPassword(plainTextPassword, costParameter);
        }
    }
}
