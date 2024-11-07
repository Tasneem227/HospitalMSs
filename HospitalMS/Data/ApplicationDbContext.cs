using HospitalMS.Configurations;
using HospitalMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalMS.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Data Source=PC-1\\SQLEXPRESS;Initial Catalog=HMSystem2;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new MedicalRecordsConfigurations().Configure(modelBuilder.Entity<MedicalRecord>());
            new BookingConfigurations().Configure(modelBuilder.Entity<Booking>());
            modelBuilder.Entity<MedicalRecord>()
               .HasKey(m => m.Id);

            modelBuilder.Entity<MedicalRecord>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd(); // Enables auto-increment

            modelBuilder.Entity<DoctorNurse>()
                .HasKey(dn => new { dn.NurseId, dn.DoctorId });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DoctorNurse> DoctorsNurses { get; set; }
        
    }
}

