using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Id,
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


        builder.ComplexProperty(v => v.GeneralDescription, vb =>
        {
            vb.Property(d => d.Value)
                .HasColumnName("general_description")
                .HasMaxLength(Constants.MAX_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.AgeExperience, vb =>
        {
            vb.Property(v => v.Years)
                .HasColumnName("age_experience")
                .IsRequired();
        });

        builder.ComplexProperty(v => v.PhoneNumber, vb =>
        {
            vb.Property(p => p.Value)
                .HasColumnName("phone_number")
                .IsRequired();
        });

        builder.OwnsOne(v => v.SocialLinkList, vb =>
        {
            vb.ToJson("social_links");

            vb.OwnsMany(v => v.SocialLinks, vbs =>
            {
                vbs.Property(v => v.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH);

                vbs.Property(v => v.Url)
                    .IsRequired()
                    .HasColumnName("url");
            });
        });

        builder.OwnsOne(v => v.RequisiteList, vb =>
        {
            vb.ToJson("requisites");

            vb.OwnsMany(v => v.Requisites, vbr =>
            {
                vbr.Property(v => v.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(Constants.MIN_TEXT_LENGTH);

                vbr.Property(v => v.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(Constants.EXTRA_TEXT_LENGTH);
            });
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}