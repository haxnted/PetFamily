using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dto;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");

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

        builder.Property(p => p.Requisites)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<RequisiteDto>>(json, JsonSerializerOptions.Default)!);

        builder.Property(p => p.Photos)
            .HasConversion(
                values => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<PetPhotoDto>>(json, JsonSerializerOptions.Default)!);

        builder.HasKey(p => p.Id);
    }
}