using HospitalMS.Data;
using HospitalMS.Models;

namespace HospitalMS.Repository
{
    public class DoctorNurseRepository : IDoctorNurseRepository
    {
        
        ApplicationDbContext context;
        

        public DoctorNurseRepository(ApplicationDbContext context)
        {
            this.context = context;
            
        }
        public void Add(DoctorNurse DN)
        {
            context.DoctorsNurses.Add(DN);
        }

    

        public int GetDocIdByNrsId(int NrsId)
        {
            return context.DoctorsNurses
                .Where(n=> n.NurseId==NrsId)
                .Select(d=> d.DoctorId)
                .FirstOrDefault();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
