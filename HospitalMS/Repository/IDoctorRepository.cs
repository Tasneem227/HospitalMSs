using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public interface IDoctorRepository
    {
        public List<Doctor> GetAll();
        public List<Doctor> GetListByDeptId(int id);

        public int GetDeptIdByDocId(int DocId);

        public void AddMedicalRecordForDoctor(int id, MedicalRecord record);

        public void Add(Doctor doctor);
        public void Update(Doctor doctor);
        public void Remove(int id);
        public Doctor GetById(int id);
        public Doctor GetByIdWithMedicalRcord(int id);
        public Doctor SearchByUserName(string username);

        public void Save();

    }
}
