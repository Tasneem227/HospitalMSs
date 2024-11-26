using Microsoft.AspNetCore.Mvc;

namespace HospitalMS.ViewModel
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        public string? userid { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MinLength(3, ErrorMessage = "First name must have more than 3 letters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(3, ErrorMessage = "Last name must have more than 3 letters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(male|female|Male|Female)$", ErrorMessage = "Gender must be either male or female.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [Remote(action: "CustomBirthDateValidation", controller: "Admin", ErrorMessage = "Age must be at least 20 years old")]
        [Display(Name = "Birth Date")]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^\+?[0-9]\d{10,14}$", ErrorMessage = "Phone number must be 10 numbers or more")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must have more than 3 characters.")]
        [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        [Remote(action: "CheckUserName", controller: "Admin", ErrorMessage = "user name is used")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must have at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Password must contain at least more than 6 and one uppercase letter " +
            "\n one lowercase letter, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        //[Required(ErrorMessage = "You should upload Your Image")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Image must be a JPG or PNG file.")]
        public string? Imag { get; set; }
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Image must be a JPG or PNG file.")]
        public string? CurrentImage { get; set; }


        public string? OldPassword { get; set; }
        public string? OldUserName { get; set; }

    }
}
