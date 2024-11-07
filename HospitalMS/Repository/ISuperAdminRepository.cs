namespace HospitalMS.Repository
{
    public interface ISuperAdminRepository
    {
        public void Add(SuperAdmin superAdmin);
        public int GetByUserName(string Username);
        public List<SuperAdmin> GetAll();
        public void Save();

    }
}
