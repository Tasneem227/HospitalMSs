using HospitalMS.Data;
using HospitalMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalMS.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        ApplicationDbContext context;
        public DoctorRepository(ApplicationDbContext _context)
        {
            context = _context;
        }


        public int GetDeptIdByDocId(int docId)
        {
            // Retrieve the department ID or 0 if the doctor is not found
            return context.Doctors
                           .Where(d => d.Id == docId)
                           .Select(d => d.DepartmentId)
                           .SingleOrDefault(); // Returns 0 if no doctor is found
        }

        public List<Doctor> GetListByDeptId(int id)
        {
            return context.Doctors.Where(d => d.DepartmentId == id).ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public Doctor GetByIdWithMedicalRcord(int id)
        {
            return context.Doctors.Include(i=>i.Bookings.Where(d=>d.DoctorId==id))
                                                        .Include(o=>o.MedicalRecords.Where(d=>d.DoctorId==id))
                                                        .FirstOrDefault(i => i.Id == id);
        }
        public void Add(Doctor doctor)
        {
            context.Add(doctor);
        }

        public List<Doctor> GetAll()
        {
            return context.Doctors.Include(d=> d.Department).ToList();
        }

        public Doctor GetById(int id)
        {
            return context.Doctors.FirstOrDefault(i => i.Id == id);
        }

        public Doctor SearchByUserName(string username)
        {
            return context.Doctors.FirstOrDefault(i => i.Username == username);
        }

        public void AddMedicalRecordForDoctor(int id, MedicalRecord record)
        {
            context.Doctors.Include(d => d.MedicalRecords).FirstOrDefault(d => d.Id == id).MedicalRecords.Add(record);
        }

        public void Remove(int id)
        {
            Doctor doctor = GetById(id);
            context.Remove(doctor);
        }

        public void Update(Doctor doctor)
        {
            context.Update(doctor);
        }

    }
}
