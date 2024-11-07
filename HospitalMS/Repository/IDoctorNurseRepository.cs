using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public interface IDoctorNurseRepository
    {
        public void Add(DoctorNurse DN);
        
        public int GetDocIdByNrsId(int NrsId);
        public void Save();
    }
}
