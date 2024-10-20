using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ComplexProperty(pa => pa.FullName, pab =>
            {
                pab.Property(f => f.Name)
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("name");
                pab.Property(f => f.Surname)
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("surname");
                pab.Property(f => f.Patronymic)
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                    .HasColumnName("patronymic");
            });
    }
}
