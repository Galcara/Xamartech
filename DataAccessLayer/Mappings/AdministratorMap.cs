using Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Mappings
{
    public class AdministratorMap : IEntityTypeConfiguration<Administrator>
    {
        public void Configure(EntityTypeBuilder<Administrator> builder)
        {
            builder.ToTable("ADMINISTRATORS");

            builder.Property(e => e.AdmName).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.HasIndex(e => e.Cpf, "UQ_ADMINISTRATORS_CPF").IsUnique();
            builder.Property(e => e.Cpf).IsRequired().HasMaxLength(11).IsUnicode(false).IsFixedLength(true);

            builder.HasIndex(e => e.Email, "UQ_ADMINISTRATORS_EMAIL").IsUnique();
            builder.Property(e => e.Email).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(e => e.BirthDate).HasColumnType("date");

            builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(12).IsUnicode(false);

            builder.Property(e => e.Passcode).IsRequired().HasMaxLength(200).IsUnicode(false);
        }
    }
}



