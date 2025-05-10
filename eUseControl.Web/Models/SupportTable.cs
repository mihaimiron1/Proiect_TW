using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using eUseControl.Web.Data;

namespace eUseControl.Web.Models
{
    public class SupportTable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nume Utilizator")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Required]
        [Display(Name = "Număr de Telefon")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Mesaj")]
        public string Message { get; set; }

        [Required]
        [Display(Name = "Data Trimiterii")]
        public DateTime CreatedAt { get; set; }
    }
} 