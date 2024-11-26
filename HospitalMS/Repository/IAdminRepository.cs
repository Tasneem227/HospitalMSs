
using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public interface IAdminRepository
    {
        public void Add (Admin admin);
        public void Update (Admin admin);
        public void Remove (int id);
        public Admin GetById (int id);
        public Department getAdminDepartment(int id);
            public Admin GetByUserName (string Username);
        public Admin GetByUserNameAndId(string Username, int Id);
        public List<Admin> GetAll ();
        
        public void Save();

    }
}
