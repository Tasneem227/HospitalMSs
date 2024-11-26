using HospitalMS.Data;
using HospitalMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalMS.Repository
{
    public class PatientRepository : IPatientRepository
    {
        ApplicationDbContext context;
        public PatientRepository(ApplicationDbContext _context)
        {
            context= _context;
        }
        public void Add(Patient patient)
        {
            context.Patients.Add(patient);
        }
        public Patient GetById(int id)
        {
            return context.Patients.FirstOrDefault(p => p.Id == id);
        }

        public List<Patient> GetAll()
        {
            return context.Patients.ToList();
        }

        public List<Patient> GetAllPatientByDocId(int DocId)
        {
            var patients = (from booking in context.Bookings
                            join patient in context.Patients on booking.PatientId equals patient.Id
                            where booking.DoctorId == DocId
                            select patient).ToList();

            return patients;
        }
        public void Update(Patient patient)
        {
            context.Update(patient);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public void RemoveById(int id)
        {
            Patient patient = GetById(id);
            context.Remove(patient);
        }
        public Patient SearchByUserName(string username)
        {
            return context.Patients.FirstOrDefault(i => i.Username == username);
        }
    }
}
