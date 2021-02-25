using Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Mappings
{
    public class ClassMap : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("CLASSES");

            builder.HasIndex(e => e.ClassName, "UQ_CLASSNAME_NAME").IsUnique();
            builder.Property(c => c.ClassName).HasMaxLength(50).IsUnicode(false);

            builder.HasMany(e => e.Students).WithOne(c => c.Class);

            builder.HasMany(e => e.Lessons).WithOne(c => c.Class);
        }
    }
}

