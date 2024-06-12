using Domain.Entities;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.DbConfigurations
{
    internal class SchoolConfig : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder
                .ToTable("Schools", SchemaNames.Academics)
                .IsMultiTenant();

            builder
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }

    internal class TeacherConfig : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder
                .ToTable("Teachers", SchemaNames.Academics);

            builder
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
