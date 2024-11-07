using HospitalMS.Data;
using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        ApplicationDbContext context;
        public DepartmentRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public void Add(Department department)
        {
            context.Add(department);
        }

        public List<Department> GetAll()
        {
            return context.Departments.ToList();
        }

        public Department GetById(int id)
        {
            return context.Departments.FirstOrDefault(d => d.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Department department)
        {
            context.Update(department);
        }
    }
}
