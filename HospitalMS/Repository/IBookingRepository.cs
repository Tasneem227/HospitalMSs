using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public interface IBookingRepository
    {

        public List<Booking> GetNurseBookingList(int NurseId);

        public List<Booking> GetDoctorBookingList(int id);

        public List<Booking> GetPatientAppointmentList(int id);

        public Booking GetAppointment(int DocId, DateOnly dateOnly, TimeOnly timeOnly);
        public void DeletePatientAppointmentList(int Patientid);
        public List<Booking> GetBookingListByPatientId(int id);
        public List<Booking> GetBookingListByDocId(int DocId);

        public List<Booking> GetDocBookingListWithPatients(int DocId);
        public  Task<List<BookingNurseViewModel>> GetDepartmentAppointments(int departmentId);
        public Task<List<MedicalRecordNurseViewModel>> GetDepartmenMedicalRecord(int departmentId);
        public void Save();
    }
}
