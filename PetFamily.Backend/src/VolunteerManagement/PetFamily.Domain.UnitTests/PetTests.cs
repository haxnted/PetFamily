using FluentAssertions;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain;
using PetFamily.VolunteerManagement.Domain.Entities;
using PetFamily.VolunteerManagement.Domain.Enums;
using PetFamily.VolunteerManagement.Domain.ValueObjects;
using Xunit;

namespace PetFamily.Domain.UnitTests;

public class PetTests
{
    [Fact]
    public void ChangePetPosition_ShouldReturnError_WhenVolunteerHaveOnePet()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(1);
        
        // Act
        var result = volunteer.MovePet(volunteer.Pets[0].Id, 2);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be("Insufficient number of pets to complete the operation");
    }
    
    [Fact]
    public void ChangePetPosition_ShouldReturnError_WhenNewIdxIsOutOfRange()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);

        // Act
        var result = volunteer.MovePet(volunteer.Pets[0].Id, -1);

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
        var result = volunteer.MovePet(nonExistentPetId, 2);

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
        var currentPos = pet.Position;

        // Act
        var result = volunteer.MovePet(pet.Id, currentPos);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.Position.Should().Be(currentPos);
    }

    [Fact]
    public void ChangePetPosition_ShouldShiftPositionsDown_WhenNewIdxIsLessThanCurrentPosition()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);
        var pet = volunteer.Pets[2]; 

        // Act
        var result = volunteer.MovePet(pet.Id, 1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[0].Position.Value.Should().Be(2);
        volunteer.Pets[1].Position.Value.Should().Be(3);
        volunteer.Pets[2].Position.Value.Should().Be(1);
    }

    [Fact]
    public void ChangePetPosition_ShouldShiftPositionsUp_WhenNewIdxIsGreaterThanCurrentPosition()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);
        var pet = volunteer.Pets[0]; 

        // Act
        var result = volunteer.MovePet(pet.Id, 3);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[1].Position.Value.Should().Be(1);
        volunteer.Pets[2].Position.Value.Should().Be(2);
        volunteer.Pets[0].Position.Value.Should().Be(3);
    }
    [Fact]
    public void ChangePetPosition_ShouldMovePetToFirstPosition()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);
        var pet = volunteer.Pets[2]; 

        // Act
        var result = volunteer.MovePet(pet.Id, 1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[0].Position.Value.Should().Be(2);
        volunteer.Pets[1].Position.Value.Should().Be(3);
        volunteer.Pets[2].Position.Value.Should().Be(1);
    }

    [Fact]
    public void ChangePetPosition_ShouldMovePetToLastPosition()
    {
        // Arrange
        var volunteer = CreateVolunteerWithPets(3);
        var pet = volunteer.Pets[0]; // текущая позиция = 1

        // Act
        var result = volunteer.MovePet(pet.Id, 3);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[0].Position.Value.Should().Be(3);
        volunteer.Pets[1].Position.Value.Should().Be(1);
        volunteer.Pets[2].Position.Value.Should().Be(2);
    }

    private Volunteer CreateVolunteerWithPets(int petCount)
    {
        var volunteer = new Volunteer(
            VolunteerId.NewId(),
            FullName.Create("John", "Doe", "sdfsfws").Value,
            Description.Create("General Description").Value,
            AgeExperience.Create(5).Value,
            PhoneNumber.Create("7234567890").Value,
            [],
            []
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
                [],
                []
            );
            volunteer.AddPet(pet);
        }

        return volunteer;
    }
    
}
