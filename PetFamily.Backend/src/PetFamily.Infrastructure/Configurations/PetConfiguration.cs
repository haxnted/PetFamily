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
            .HasMaxLength(Constants.MIN_TEXT_LENGTH)
            .IsRequired();
        
        builder.Property(p => p.TypeAnimal)
            .IsRequired();

        builder.Property(p => p.GeneralDescription)
            .HasConversion(
                value => value.Value,
                res => Description.Create(res).Value
            )
            .HasMaxLength(Constants.EXTRA_TEXT_LENGTH)
            .IsRequired();
        
        builder.Property(p => p.Breed).IsRequired();
        
        builder.Property(p => p.Color).IsRequired();
        
        builder.Property(p => p.HealthInformation)
            .HasConversion(
                value => value.Value,
                res => Description.Create(res).Value
            )
            .HasMaxLength(Constants.EXTRA_TEXT_LENGTH)
            .IsRequired();

        builder.ComplexProperty(p => p.Address, pb =>
        {
            pb.Property(p => p.Street)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("Street");
            
            pb.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("city");
            
            pb.Property(p => p.State)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("state");

            pb.Property(p => p.ZipCode)
                .IsRequired()
                .HasMaxLength(Constants.MIN_TEXT_LENGTH)
                .HasColumnName("zipcode");
        });

        builder.OwnsOne(p => p.PhysicalAttributes, pb =>
        {
            pb.ToJson();

            pb.Property(p => p.Weight)
                .IsRequired();

            pb.Property(p => p.Height)
                .IsRequired();

        });
        
        builder.Property(p => p.PhoneNumber)
            .HasConversion(
                number => number.Value,
                value => PhoneNumber.Create(value).Value);
        
        builder.Property(p => p.BirthDate).IsRequired();
        
        builder.Property(p => p.IsCastrated).IsRequired();
        
        builder.Property(p => p.IsVaccinated).IsRequired();
        
        builder.OwnsOne(p => p.Requisite, pv =>
        {
            pv.ToJson();

            pv.Property(p => p.RequisiteName)
                .IsRequired();

            pv.Property(p => p.RequisiteDescription)
                .IsRequired();
        });
        
        builder.Property(p => p.DateCreated).IsRequired();

        builder.OwnsOne(p => p.Details, pd =>
        {
            pd.ToJson();

            pd.OwnsMany(d => d.Photos, db =>
            {
                db.Property(p => p.Path)
                    .IsRequired();

                db.Property(p => p.IsImageMain)
                    .IsRequired();
            });

        });
    }
}