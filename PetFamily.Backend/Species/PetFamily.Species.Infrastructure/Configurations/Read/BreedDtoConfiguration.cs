using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dto;

namespace PetFamily.Species.Infrastructure.Configurations.Read;

public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.Id)
            .HasName("id");

        builder.Property(b => b.SpeciesId)
            .HasColumnName("species_fk_id");

        builder.Property(b => b.Name)
            .HasColumnName("breed");
    }
}