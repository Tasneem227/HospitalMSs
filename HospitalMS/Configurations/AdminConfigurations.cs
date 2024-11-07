using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HospitalMS.Configurations
{
    public class AdminConfigurations : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasOne(a => a.SuperAdmin)
        .WithMany()                
        .HasForeignKey(a => a.SuperAdminId)
        .IsRequired(false);
           
        }
    }
}
