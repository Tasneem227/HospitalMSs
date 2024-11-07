
namespace HospitalMS.Repository
{
    public class SuperAdminRepository : ISuperAdminRepository
    {
        private readonly ApplicationDbContext context;

        public SuperAdminRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(SuperAdmin admin)
        {
            context.Add(admin);
        }

        public List<SuperAdmin> GetAll()
        {
            return context.SuperAdmins.ToList();
        }

        public int GetByUserName(string Username)
        {
            return context.SuperAdmins.FirstOrDefault(i => i.Username == Username).Id;
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
