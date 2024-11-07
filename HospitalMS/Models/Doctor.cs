using System.ComponentModel.DataAnnotations;

namespace HospitalMS.Models
{
    public class Doctor
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "First name must have more than 3 letters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Last name must have more than 3 letters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required]

        [RegularExpression("^(male|female|Male|Female)$", ErrorMessage = "Gender must be either male or female.")]

        public string Gender { get; set; }

        public DateOnly BirthDate { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^\+?[0-9]\d{10,14}$", ErrorMessage = "Phone number must be 10 numbers or more")]

        public string Phone { get; set; }


        public string Username { get; set; }


        public string Password { get; set; }

        [Required]
        public int Salary { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Image must be a JPG or PNG file.")]
        public string? Imag { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public Department? Department { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

        public List<Admin>? Admins { get; set; }

        public List<Nurse>? Nurses { get; set; }

        public List<DoctorNurse>? DoctorNurse { get; set; }

        public List<MedicalRecord>? MedicalRecords { get; set; }
    }
}