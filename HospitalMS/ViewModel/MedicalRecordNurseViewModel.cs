namespace HospitalMS.ViewModel
{
    public class MedicalRecordNurseViewModel
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string Note { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string DoctorImage { get; set; }

    }
}
