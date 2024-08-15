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

        builder.ComplexProperty(v => v.FullName, vb =>
        {
            vb.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("name");
            
            vb.Property(v => v.Surname)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("surname");
            
            vb.Property(v => v.Patronymic)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("patronymic");
        });
        
        
        builder.Property(v => v.GeneralDescription)
            .HasConversion(
                description => description.Value,
                value => Description.Create(value).Value
                )
            .HasMaxLength(Constants.MAX_TEXT_LENGTH)
            .IsRequired();

        builder.ComplexProperty(v => v.AgeExperience, vb =>
        {
            vb.Property(v => v.Years)
                .IsRequired();
            vb.Property(v => v.Months)
                .IsRequired();
        });

        builder.Property(v => v.PhoneNumber)
            .HasConversion(
                number => number.Value,
                result => PhoneNumber.Create(result).Value
            );

        builder.Property(v => v.PetsAdoptedCount);
        
        builder.Property(v => v.PetsFoundHomeQuantity);
        
        builder.Property(v => v.PetsUnderTreatmentCount);

        builder.OwnsOne(v => v.Details, vb =>
        {
            vb.ToJson();
            vb.OwnsMany(v => v.SocialLinks, vb =>
            {
                vb.Property(v => v.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH);
            
                vb.Property(v => v.Url)
                    .IsRequired()
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH);
            });
        
            vb.OwnsMany(v => v.Requisites, vb =>
            {
                vb.Property(v => v.RequisiteName)
                    .IsRequired()
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH);
            
                vb.Property(v => v.RequisiteDescription)
                    .IsRequired()
                    .HasMaxLength(Constants.EXTRA_TEXT_LENGTH);
            });
        });

        
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id");

        
        

    }
}