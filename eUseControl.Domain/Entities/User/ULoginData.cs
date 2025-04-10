using System;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Domain.Entities.User
{
    public class ULoginData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string LoginIp { get; set; }

        public DateTime LoginDateTime { get; set; }

        public bool Success { get; set; }
    }
}
