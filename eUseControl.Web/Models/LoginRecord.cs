using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Web.Models
{
    [Table("LoginRecords")]
    public class LoginRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        [Required]
        public bool Success { get; set; }

        [StringLength(45)] // IPv6 addresses can be up to 45 characters
        public string IPAddress { get; set; }
    }
} 