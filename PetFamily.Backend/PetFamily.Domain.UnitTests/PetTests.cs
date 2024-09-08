using FluentAssertions;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.UnitTests;

public class PetTests
{
    [Fact]
    public void ChangePetPosition_ShouldReturnError_WhenNewIdxIsOutOfRange()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);

        // Act
        var result = volunteer.ChangePetPosition(volunteer.Pets[0].Id, -1);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be("newIdx is invalid");
    }

    [Fact]
    public void ChangePetPosition_ShouldReturnError_WhenPetNotFound()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);
        var nonExistentPetId = PetId.NewId();

        // Act
        var result = volunteer.ChangePetPosition(nonExistentPetId, 2);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be($"record not found for Id '{nonExistentPetId.Id}'");
    }

    [Fact]
    public void ChangePetPosition_ShouldSucceed_WhenNewIdxIsTheSameAsCurrentPosition()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);
        var pet = volunteer.Pets[0];
        var currentPos = pet.SerialNumber;

        // Act
        var result = volunteer.ChangePetPosition(pet.Id, currentPos);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.SerialNumber.Should().Be(currentPos);
    }

    [Fact]
    public void ChangePetPosition_ShouldShiftPositionsDown_WhenNewIdxIsLessThanCurrentPosition()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);
        var pet = volunteer.Pets[2]; 

        // Act
        var result = volunteer.ChangePetPosition(pet.Id, 1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.SerialNumber.Should().Be(1);
        volunteer.Pets[0].SerialNumber.Should().Be(2);
        volunteer.Pets[1].SerialNumber.Should().Be(3);
    }

    [Fact]
    public void ChangePetPosition_ShouldShiftPositionsUp_WhenNewIdxIsGreaterThanCurrentPosition()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);
        var pet = volunteer.Pets[0]; 

        // Act
        var result = volunteer.ChangePetPosition(pet.Id, 3);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.SerialNumber.Should().Be(3);
        volunteer.Pets[1].SerialNumber.Should().Be(1);
        volunteer.Pets[2].SerialNumber.Should().Be(2);
    }
    
    private Volunteer CreateVolunteerWithPets(int petCount)
    {
        var volunteer = new Volunteer(
            VolunteerId.NewId(),
            FullName.Create("John", "Doe", "sdfsfws").Value,
            Description.Create("General Description").Value,
            AgeExperience.Create(5).Value,
            PhoneNumber.Create("7234567890").Value,
            new ValueObjectList<SocialLink>(new List<SocialLink>()),
            new ValueObjectList<Requisite>(new List<Requisite>())
        );

        for (int i = 0; i < petCount; i++)
        {
            var pet = new Pet(
                PetId.NewId(),
                NickName.Create($"Pet {i + 1}").Value,
                Description.Create("General Description").Value,
                Description.Create("Health Information").Value,
                Address.Create("address", "address", "address", "address").Value,
                PetPhysicalAttributes.Create(10, 20).Value,
                Guid.NewGuid(),
                BreedId.NewId(),
                PhoneNumber.Create("7234567890").Value,
                DateTime.Now.AddYears(-1),
                true,
                true,
                HelpStatusPet.LookingForHome,
                DateTime.Now,
                new ValueObjectList<PetPhoto>(new List<PetPhoto>()),
                new ValueObjectList<Requisite>(new List<Requisite>())
            );
            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}

