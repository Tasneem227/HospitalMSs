using System.ComponentModel.DataAnnotations;

namespace HospitalMS.Models
{
    public class Patient
    {
        [Key]
        [Required]
        
        public int Id { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
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
        [Required]
       public string? Imag { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public List<Nurse> Nurses { get; set; }

        public List<MedicalRecord> MedicalRecords { get; set; }
       


    }
}
