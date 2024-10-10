using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.Species.Domain.Entities;

namespace PetFamily.Species.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Id,
                result => BreedId.Create(result)
            );

        builder.Property(b => b.SpeciesId)
            .HasConversion(
                id => id.Id,
                result => SpeciesId.Create(result)
            )
            .HasColumnName("species_fk_id");
        
        builder.Property(b => b.Value)
            .HasColumnName("breed")
            .IsRequired();
    }
}