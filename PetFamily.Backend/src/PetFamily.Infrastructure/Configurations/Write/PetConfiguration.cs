using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dto;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations.Write;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Id,
                value => PetId.Create(value));

        builder.ComplexProperty(p => p.NickName, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("nick_name")
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.GeneralDescription, vb =>
        {
            vb.Property(d => d.Value)
                .HasMaxLength(Constants.EXTRA_TEXT_LENGTH)
                .HasColumnName("general_description")
                .IsRequired();
        });

        builder.Property(p => p.SpeciesId)
            .HasColumnName("species_id");

        builder.ComplexProperty(p => p.BreedId, pb =>
        {
            pb.Property(p => p.Id)
                .HasColumnName("breed_id");
        });

        builder.ComplexProperty(p => p.HealthInformation, vb =>
        {
            vb.Property(d => d.Value)
                .HasMaxLength(Constants.EXTRA_TEXT_LENGTH)
                .HasColumnName("health_information")
                .IsRequired(false);
        });

        builder.ComplexProperty(p => p.Address, pb =>
        {
            pb.Property(p => p.Street)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("street");

            pb.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("city");

            pb.Property(p => p.State)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("state");

            pb.Property(p => p.ZipCode)
                .IsRequired(false)
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("zipcode");
        });

        builder.ComplexProperty(p => p.PhysicalAttributes, pb =>
        {
            pb.Property(p => p.Weight)
                .HasColumnName("weight")
                .IsRequired();

            pb.Property(p => p.Height)
                .HasColumnName("height")
                .IsRequired();
        });

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("phone_number")
                .IsRequired();
        });

        builder.Property(p => p.BirthDate)
            .HasColumnName("birth_date")
            .IsRequired();

        builder.Property(p => p.IsCastrated)
            .HasColumnName("is_castrated")
            .IsRequired();

        builder.Property(p => p.IsVaccinated)
            .HasColumnName("is_vaccinated")
            .IsRequired();

        builder.Property(p => p.DateCreated)
            .HasColumnName("date_created")
            .IsRequired();

        builder.Property(v => v.PetPhotoList)
            .HasValueObjectsJsonConversion(
                input => new PetPhotoDto() { Path = input.Path },
                output => PetPhoto.Create(FilePath.Create(output.Path).Value, output.IsPhotoMain).Value)
            .HasColumnName("requisites");
        
        builder.Property(v => v.RequisiteList)
            .HasValueObjectsJsonConversion(
                input => new RequisiteDto(input.Name, input.Description),
                output => Requisite.Create(output.Name, output.Description).Value)
            .HasColumnName("requisites");

        builder.ComplexProperty(p => p.Position, pb =>
        {
            pb.Property(p => p.Value)
                .HasColumnName("serial_number")
                .IsRequired();
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}