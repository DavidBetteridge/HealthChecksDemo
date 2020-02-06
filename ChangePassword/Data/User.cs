using System.ComponentModel.DataAnnotations;

namespace ChangePassword.Data
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public string Username { get; set; } = default!;

        [Required]
        [MaxLength(300)]
        public string PasswordHash { get; set; } = default!;
    }
}
