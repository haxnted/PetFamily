using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public  class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Id,
                value => PetId.Create(value));
        
        builder.Property(p => p.NickName)
            .HasMaxLength(Pet.MIN_TEXT_LENGTH)
            .IsRequired();
        
        builder.Property(p => p.TypeAnimal)
            .IsRequired();
        
        builder.Property(p => p.GeneralDescription)
            .HasMaxLength(Pet.MAX_TEXT_LENGTH)
            .IsRequired();
        
        builder.Property(p => p.Breed).IsRequired();
        
        builder.Property(p => p.Color).IsRequired();
        
        builder.Property(p => p.PetHealthInformation)
            .IsRequired()
            .HasMaxLength(Pet.MAX_TEXT_LENGTH);
        
        builder.Property(p => p.Address).IsRequired();
        
        builder.Property(p => p.Weight).IsRequired();
        
        builder.Property(p => p.Height).IsRequired();
        
        builder.Property(p => p.PhoneNumber).IsRequired();
        
        builder.Property(p => p.BirthDate).IsRequired();
        
        builder.Property(p => p.IsCastrated).IsRequired();
        
        builder.Property(p => p.IsVaccinated).IsRequired();
        
        builder.OwnsOne(p => p.Requisites, pv =>
        {
            pv.ToJson();

            pv.Property(p => p.RequisitesName)
                .IsRequired();

            pv.Property(p => p.RequisitesDescription)
                .IsRequired();
        });
        
        builder.Property(p => p.DateCreated).IsRequired();
        
        builder.HasMany(p => p.Photos)
            .WithOne()
            .HasForeignKey("pet_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}