using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public interface INurseRepository
    {

        public int GetLastInsertedId();
        public void Add(Nurse nurse);
        public void Update(Nurse nurse);
        public void RemoveById(int id);
        public Nurse GetById(int id);
        public Nurse SearchByUserName(string username);
        public List<Nurse> GetAll();
        public List<Nurse> GetByDeptId(int DeptId);
        

        public void Save();
    }
}
