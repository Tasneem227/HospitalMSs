namespace HospitalMS.Models
{
    public class DoctorNurse
    {
        public int DoctorId { get; set; }
        public int NurseId { get; set; }
        public Doctor doctor { get; set; }
        public Nurse nurse { get; set; }
    }
}
