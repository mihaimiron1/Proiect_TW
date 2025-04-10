using System;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Domain.Entities.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
} 