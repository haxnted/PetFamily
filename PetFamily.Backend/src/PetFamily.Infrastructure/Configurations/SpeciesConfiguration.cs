﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Аggregate.Species;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                speciesId => speciesId.Id,
                result => SpeciesId.Create(result)
            );

        builder.Property(x => x.TypeAnimal)
            .IsRequired();

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("species_fk_id");
    }
}