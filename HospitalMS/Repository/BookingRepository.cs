using HospitalMS.Data;
using HospitalMS.Models;
using Microsoft.EntityFrameworkCore;
using static HospitalMS.ViewModel.BookingNurseViewModel;

namespace HospitalMS.Repository
{
    public class BookingRepository : IBookingRepository
    {
        ApplicationDbContext context;
        public BookingRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Booking GetAppointment(int DocId, DateOnly dateOnly, TimeOnly timeOnly)
        {
          
            return context.Bookings.FirstOrDefault(b => b.DoctorId == DocId
                                                   && b.Date.Year==dateOnly.Year
                                                   &&b.Date.Month==dateOnly.Month
                                                   &&b.Date.Day==dateOnly.Day
                                                   && b.Time.Hour==timeOnly.Hour
                                                   &&b.Time.Minute==timeOnly.Minute
                                                   &&b.Time.Second==timeOnly.Second );
        }

        public List<Booking> GetDoctorBookingList(int id)
        {
            return context.Bookings.Where(b => b.DoctorId == id && b.PatientId == null).ToList();
        }

        public List<Booking> GetPatientAppointmentList(int id)
        {
            return context.Bookings.Where(b => b.PatientId == id).Include(b => b.Doctor).ToList();
        }

        public List<Booking> GetBookingListByPatientId(int id)
        {
            return context.Bookings.Where(b => b.PatientId == id).Include(b => b.Doctor).ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<Booking> GetDocBookingListWithPatients(int DocId)
        {
            return context.Bookings.Where(b => b.DoctorId == DocId && b.PatientId != null).Include(b => b.Doctor).Include(p=>p.Patient).Include(d=>d.Department).ToList();
        }

        public List<Booking> GetNurseBookingList(int NurseId)
        {
            return context.Bookings.Include(d=>d.Doctor).ToList();
        }
        public List<Booking> GetBookingListByDocId(int DocId)
        {

            return context.Bookings
                .Include(b => b.Patient)
                .Where(b => b.DoctorId == DocId)
                .ToList();

        }
        public async Task<List<BookingNurseViewModel>> GetDepartmentAppointments(int departmentId)
        {
            if (departmentId <= 0)
                throw new ArgumentException("Invalid Department ID", nameof(departmentId));

                return await context.Bookings
                .Include(b => b.Patient)
                .Include(d => d.Doctor)
                .Where(b => b.DepartmentId == departmentId&&b.PatientId!=null &&b.Doctor.DepartmentId==departmentId)
                .Select(b => new BookingNurseViewModel
                 {
                    PatientFirstName=b.Patient.FName,
                    PatientLastName = b.Patient.LName,
                    DoctorFirstName = b.Doctor.FName,
                    DoctorLastName = b.Doctor.LName,
                    Time = b.Time,
                    Date = b.Date,
                    DoctorImage = b.Doctor.Imag

                })
                .ToListAsync();
        }
        public async Task<List<MedicalRecordNurseViewModel>> GetDepartmenMedicalRecord(int departmentId)
        {
            if (departmentId <= 0)
                throw new ArgumentException("Invalid Department ID", nameof(departmentId));

            return await context.MedicalRecords
            .Include(d => d.Doctor)
            .ThenInclude(d => d.MedicalRecords).ThenInclude(m => m.Patient)
            .Where(b => b.Doctor.DepartmentId == departmentId)
            .Select(b => new MedicalRecordNurseViewModel
            {
                PatientFirstName = b.Patient.FName,
                PatientLastName = b.Patient.LName,
                DoctorFirstName = b.Doctor.FName,
                DoctorLastName = b.Doctor.LName,
                Date=b.Date,
                Note = b.Note,
                DoctorImage=b.Doctor.Imag

            })
            .ToListAsync();
        }
    }
}
