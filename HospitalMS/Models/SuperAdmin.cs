using System.ComponentModel.DataAnnotations;

namespace HospitalMS.Models
{
    public class SuperAdmin
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public string Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Imag { get; set; }

        public List<Admin> Admins { get; set; }

    }
}
