using Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Mappings
{
    public class SubjectMap : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("SUBJECTS");

            builder.HasIndex(e => e.SubjectName, "UQ_SUBJECTNAME_NAME").IsUnique();
            builder.Property(e => e.SubjectName).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(e => e.Frequency).IsUnicode(false);


            builder.HasMany(e => e.Teachers).WithMany(e => e.Subjects);

            builder.HasMany(e => e.Lessons).WithOne(c => c.Subject);
        }
    }
}