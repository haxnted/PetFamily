using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Infrastructure.Configurations;

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

        builder.OwnsOne(p => p.PetPhotoList, pb =>
        {
            pb.ToJson("pet_photos");

            pb.OwnsMany(d => d.PetPhotos, db =>
            {
                db.Property(p => p.Path)
                    .IsRequired()
                    .HasColumnName("path");

                db.Property(p => p.IsImageMain)
                    .IsRequired()
                    .HasColumnName("is_image_main");
            });
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}