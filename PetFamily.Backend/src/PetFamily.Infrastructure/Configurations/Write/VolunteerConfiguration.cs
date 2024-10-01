using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dto;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations.Write;

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

        builder.Property(v => v.SocialLinkList)
            .HasValueObjectsJsonConversion(
                input => new SocialLinkDto(input.Name, input.Url),
                output => SocialLink.Create(output.Name, output.Url).Value)
            .HasColumnName("social_links");

        builder.Property(v => v.RequisiteList)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto(input.Name, input.Description),
                output => Requisite.Create(output.Name, output.Description).Value)
            .HasColumnName("requisites");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey(v => v.VolunteerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}