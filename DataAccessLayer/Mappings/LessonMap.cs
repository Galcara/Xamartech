using Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Mappings
{
    public class LessonMap : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("LESSONS");

            builder.HasOne(d => d.Teacher).WithMany(p => p.Lessons).HasForeignKey(d => d.TeacherID).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_LESSON_TEACHER");

            builder.HasOne(d => d.Subject).WithMany(d => d.Lessons).HasForeignKey(d => d.SubjectID).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_LESSON_SUBJECT");

            builder.HasMany(c => c.Presences).WithOne(c => c.Lesson);
        }
    }
}