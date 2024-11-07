using System.ComponentModel.DataAnnotations;

namespace HospitalMS.Models
{
    public class MedicalRecord
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please Enter Note For This Patient")]
        [Display(Name = "Enter Notes For Patient")]

        public string Note { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        [Display(Name = "Enter Patient Id")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
