using Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Mappings
{
    public class TeacherMap : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("TEACHERS");

            builder.HasIndex(e => e.Cpf, "UQ_TEACHERS_CPF").IsUnique();
            builder.Property(e => e.Cpf).IsRequired().HasMaxLength(11).IsUnicode(false).IsFixedLength(true);

            builder.HasIndex(e => e.Email, "UQ_TEACHERS_EMAIL").IsUnique();
            builder.Property(e => e.Email).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(e => e.BirthDate).HasColumnType("date");

            builder.Property(e => e.Passcode).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(12).IsUnicode(false);

            builder.Property(e => e.TeacherName).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.HasMany(c => c.Subjects).WithMany(c => c.Teachers);

            builder.HasMany(c => c.Lessons).WithOne(c => c.Teacher);
        }
    }
}