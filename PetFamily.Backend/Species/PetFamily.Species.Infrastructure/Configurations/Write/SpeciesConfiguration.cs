using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.EntityIds;

namespace PetFamily.Species.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Domain.Species>
{
    public void Configure(EntityTypeBuilder<Domain.Species> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                speciesId => speciesId.Id,
                result => SpeciesId.Create(result)
            );

        builder.ComplexProperty(s => s.TypeAnimal, sb =>
        {
            sb.Property(s => s.Value)
                .HasColumnName("type_animal")
                .IsRequired();
        });

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey(s => s.SpeciesId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}