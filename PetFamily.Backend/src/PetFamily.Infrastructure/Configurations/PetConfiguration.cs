using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Аggregate.Volunteer;

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

        builder.Property(p => p.NickName)
            .HasMaxLength(Constants.MIN_TEXT_LENGTH)
            .IsRequired();

        builder.ComplexProperty(p => p.GeneralDescription, vb =>
        {
            vb.Property(d => d.Value)
                .HasMaxLength(Constants.EXTRA_TEXT_LENGTH)
                .HasColumnName("general_description")
                .IsRequired();
        });

        builder.ComplexProperty(p => p.SpeciesId, pb =>
        {
            pb.Property(p => p.Id)
                .HasColumnName("species_id");
        });

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

        builder.Property(p => p.BirthDate).IsRequired();

        builder.Property(p => p.IsCastrated).IsRequired();

        builder.Property(p => p.IsVaccinated).IsRequired();

        builder.Property(p => p.DateCreated).IsRequired();

        builder.OwnsOne(p => p.Details, pd =>
        {
            pd.ToJson();

            pd.OwnsMany(d => d.PetPhotos, db =>
            {
                db.Property(p => p.Path)
                    .IsRequired();

                db.Property(p => p.IsImageMain)
                    .IsRequired();
            });

            pd.OwnsMany(p => p.Requisites, pb =>
            {
                pb.Property(p => p.RequisiteName)
                    .IsRequired();

                pb.Property(p => p.RequisiteDescription)
                    .IsRequired();
            });
        });
    }
}