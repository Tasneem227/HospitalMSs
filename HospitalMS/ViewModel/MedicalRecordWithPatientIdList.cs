using HospitalMS.Data;
using HospitalMS.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalMS.ViewModel
{

    public class MedicalRecordWithPatientIdList
    {

        [Key]
        [Required]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;



        public string Note { get; set; }

        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public List<Patient> patientlist { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string FullName => $"{FName} {LName}";
        public ICollection<Booking> Bookings { get; set; }
        public List<MedicalRecord> MedicalRecords { get; set; }










    }
}
