using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id=> id.Id,
                value => VolunteerId.Create(value));
        
        builder.Property(v => v.Name)
            .HasMaxLength(Volunteer.MAX_LENGTH_USERNAME)
            .IsRequired();
        
        builder.Property(v => v.Surname)
            .HasMaxLength(Volunteer.MAX_LENGTH_USERNAME)
            .IsRequired();
        
        builder.Property(v => v.Patronymic)
            .HasMaxLength(Volunteer.MAX_LENGTH_USERNAME)
            .IsRequired();
        
        builder.Property(v => v.GeneralDescription)
            .HasMaxLength(Volunteer.MAX_GENERAL_DESCRIPTION)
            .IsRequired();

        builder.Property(v => v.AgeExperience)
            .IsRequired();

        builder.Property(v => v.PhoneNumber)
            .IsRequired();

        builder.Property(v => v.PetsAdoptedCount);
        
        builder.Property(v => v.PetsFoundHomeQuantity);
        
        builder.Property(v => v.PetsUnderTreatmentCount);

        builder.OwnsMany(v => v.SocialLinks, vb =>
        {
            vb.ToJson();
            vb.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(SocialLink.MIN_TEXT_NAME);
            
            vb.Property(v => v.Url)
                .IsRequired()
                .HasMaxLength(SocialLink.MAX_TEXT_URL);
        });
        
        builder.OwnsMany(v => v.Requisites, vb =>
        {
            vb.ToJson();
            vb.Property(v => v.RequisitesName)
                .IsRequired()
                .HasMaxLength(SocialLink.MIN_TEXT_NAME);
            
            vb.Property(v => v.RequisitesDescription)
                .IsRequired()
                .HasMaxLength(SocialLink.MAX_TEXT_URL);
        });
        
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id");

        
        

    }
}