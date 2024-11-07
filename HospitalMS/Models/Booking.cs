using System.ComponentModel.DataAnnotations;

namespace HospitalMS.Models
{
    public class Booking
    {
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public int? PatientId { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        public Department Department { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
