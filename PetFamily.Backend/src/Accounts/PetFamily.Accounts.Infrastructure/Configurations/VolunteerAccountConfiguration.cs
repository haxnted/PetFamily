using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;


namespace PetFamily.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
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
        
        builder.Property(v => v.RequisiteList)
            .HasConversion(
                u => JsonSerializer.Serialize(u, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<Requisite>>(json, JsonSerializerOptions.Default)!,
                new ValueComparer<List<Requisite>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                    c => c.ToList()))
            .HasColumnName("requisites");
    }
}
