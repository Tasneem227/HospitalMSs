using HospitalMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HospitalMS.Configurations
{
    public class MedicalRecordsConfigurations : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder
                .HasKey(m => new { m.Id, m.DoctorId, m.PatientId });
        }
    }
}
