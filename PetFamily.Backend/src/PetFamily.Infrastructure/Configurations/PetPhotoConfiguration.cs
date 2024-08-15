using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;

namespace PetFamily.Infrastructure.Configurations;

public class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
{
    public void Configure(EntityTypeBuilder<PetPhoto> builder)
    {
        builder.HasKey(p => p.Id);
    }
}