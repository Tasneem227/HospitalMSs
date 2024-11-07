using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public interface IDepartmentRepository
    {
        public List<Department> GetAll();

        public Department GetById(int id);

        public void Add(Department department);

        public void Update(Department department);

        public void Save();

    }
}
