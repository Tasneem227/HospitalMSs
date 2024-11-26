using HospitalMS.Data;
using HospitalMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalMS.Repository
{
    public class AdminRepository : IAdminRepository
    {

        ApplicationDbContext context;
        public AdminRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(Admin admin)
        {
            context.Add(admin);
        }

        public List<Admin> GetAll()
        {
            return context.Admins.Include(d=>d.Department).ToList();
        }

        public Admin GetById(int id)
        {
            return context.Admins.Include(d => d.Department).FirstOrDefault(i=>i.Id == id);
        }
        public Department getAdminDepartment(int id) {
            return context.Departments.FirstOrDefault(i => i.Id == id);
        }
        public Admin GetByUserName(string Username)
        {
            return context.Admins.FirstOrDefault(i=>i.Username == Username);
        }

        public Admin GetByUserNameAndId(string Username, int Id)
        {
            return context.Admins.FirstOrDefault(i => i.Username == Username && i.Id==Id);
        }
        

        public void Remove(int id)
        {
            Admin admin = GetById(id);
            context.Remove(admin);
        }

        public void Save()
        {
           context.SaveChanges();
        }

        public void Update(Admin admin)
        {
            context.Update(admin);
        }
    }
}
