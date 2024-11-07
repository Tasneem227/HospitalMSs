using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public interface IMedicalRecordRepository
    {
        public List<MedicalRecord> GetAll();

        public List<MedicalRecord> GetPatientMedicalRecords(int PatientId);

        public List<MedicalRecord> GetListByMedicalRecordId(int id);

        public MedicalRecord GetById(int id);

        public List<MedicalRecord> GetMedicalRecordsByDocId(int DocId);


        public void Add(MedicalRecord record);


        public void Delete(MedicalRecord record);

        public void Save();
      
    }
}
