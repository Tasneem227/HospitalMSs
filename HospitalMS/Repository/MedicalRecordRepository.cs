using HospitalMS.Data;
using HospitalMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalMS.Repository
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        ApplicationDbContext context;
        public MedicalRecordRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public List<MedicalRecord> GetAll()
        {
            return context.MedicalRecords.ToList();
        }

        public List<MedicalRecord> GetMedicalRecordsByDocId(int DocId)
        {
            return context.MedicalRecords.Where(m => m.DoctorId == DocId && m.PatientId != null).Include(m=> m.Patient).ToList();
        }

        public List<MedicalRecord> GetListByMedicalRecordId( int id)
        {
          return  context.MedicalRecords.Where(e => e.Id == id).ToList();
        }
        public MedicalRecord GetById(int id)
        {
            return context.MedicalRecords.FirstOrDefault(e => e.PatientId == id);
        }


        public  List<MedicalRecord> GetPatientMedicalRecords(int PatientId)
        {
            return context.MedicalRecords.Where(e => e.PatientId == PatientId).Include(m=> m.Doctor).ToList();
        }

        public void Add(MedicalRecord record)
        {
            context.Add(record);
        }

        public void Delete(MedicalRecord record)
        {
            context.Remove(record);
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}
