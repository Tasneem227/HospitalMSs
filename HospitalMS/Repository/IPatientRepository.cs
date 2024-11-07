using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public interface IPatientRepository
    {
        public void Add(Patient patient);
        public List<Patient> GetAll();

        public List<Patient> GetAllPatientByDocId(int DocId);

        public Patient GetById(int id);
        public void Save();
        public Patient SearchByUserName(string username);

    }
}
