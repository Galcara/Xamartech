using Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Mappings
{
    public class StudentMap : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("STUDENTS");

            builder.HasIndex(e => e.Cpf, "UQ_STUDENTS_CPF").IsUnique();
            builder.Property(e => e.Cpf).IsRequired().HasMaxLength(11).IsUnicode(false).IsFixedLength(true);

            builder.HasIndex(e => e.Register, "UQ_STUDENTS_REGISTER").IsUnique();
            builder.Property(e => e.Register).IsRequired().HasMaxLength(30).IsUnicode(false);

            builder.Property(e => e.Passcode).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(12).IsUnicode(false);

            builder.Property(e => e.StudentName).IsRequired().HasMaxLength(70).IsUnicode(false);

            builder.HasOne(c => c.Class).WithMany(collection => collection.Students).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Lessons).WithMany(collection => collection.Students);

            builder.HasMany(c => c.Presences).WithOne(c => c.Student);
        }
    }
}