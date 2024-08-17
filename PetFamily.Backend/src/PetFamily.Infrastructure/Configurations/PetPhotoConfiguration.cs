using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
{
    public void Configure(EntityTypeBuilder<PetPhoto> builder)
    {
        builder.ToTable("pet_photos");

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Id,
                result => PetPhotoId.Create(result)
                );
        
        builder.Property(p => p.Path)
            .IsRequired();

        builder.Property(p => p.IsImageMain);
    }
}