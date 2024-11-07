using System.ComponentModel.DataAnnotations;

namespace HospitalMS.Models
{
    public class Nurse
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
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
        public int Salary { get; set; }
        [Required]
        public string? Imag { get; set; }
        [Required]


        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<Admin> Admins { get; set; }

        public List<Doctor> Doctors { get; set; }

        public List<DoctorNurse> DoctorNurse { get; set; }

        public List<Patient> Patients { get; set; }


    }
}
