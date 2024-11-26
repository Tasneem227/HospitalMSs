using System.ComponentModel.DataAnnotations;

namespace HospitalMS.Models
{
    public class Department
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string ?image { get; set; }
        [Required]
       
        public string? Description { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
        public List<Admin>? Admins { get; set; }
        public List<Nurse>? Nurses { get; set; }
        public List<Doctor>? Doctors { get; set; }



    }
}
