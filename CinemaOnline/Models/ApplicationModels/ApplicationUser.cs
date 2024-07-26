using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CinemaOnline.Models.ApplicationModels
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = null!;
    }
}
