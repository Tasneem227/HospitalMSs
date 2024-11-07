using HospitalMS.Data;
using HospitalMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalMS.Repository
{
    public class NurseRepository : INurseRepository
    {

        ApplicationDbContext context;
        public NurseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(Nurse nurse)
        {
            context.Add(nurse);
        }


        public int GetLastInsertedId()
        {
            return context.Nurses
                           .OrderByDescending(e => e.Id)
                           .Select(e => e.Id)
                           .FirstOrDefault();
        }

        public List<Nurse> GetAll()
        {
            return context.Nurses.ToList();
        }

        public Nurse GetById(int id)
        {
            return context.Nurses.FirstOrDefault(i => i.Id == id);
        }
        public Nurse SearchByUserName(string username)
        {
            return context.Nurses.FirstOrDefault(i => i.Username == username);
        }
        public void RemoveById(int id)
        {
            Nurse nurse = GetById(id);
            context.Remove(nurse);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Nurse nurse)
        {
            context.Update(nurse);
        }
        
        public List<Nurse> GetByDeptId(int DeptId)
        {
            return context.Nurses.Where(n=> n.DepartmentId == DeptId).ToList();
        }
    }
}
