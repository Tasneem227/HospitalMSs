using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace HospitalMS.Models.Identity
{
    public class ApplicationUser:IdentityUser

    {
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }

        [Required]
        public string Gender { get; set; }
        public DateOnly BirthDate { get; set; }

        public string? Image {get; set; }   
    }
}
