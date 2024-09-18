using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.Features.VolunteerManagement;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddPet;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.UnitTests;

public class AddPetTests
{
    private readonly Mock<IVolunteersRepository> _volunteerRepositoryMock;
    private readonly Mock<IValidator<AddPetCommand>> _validatorMock;
    private readonly Mock<ILogger<AddPetHandler>> _loggerMock;

    public AddPetTests()
    {
        _volunteerRepositoryMock = new Mock<IVolunteersRepository>();
        _validatorMock = new Mock<IValidator<AddPetCommand>>();
        _loggerMock = new Mock<ILogger<AddPetHandler>>();
    }

    [Fact]
    public async Task Execute_ShouldAddPet_WhenCommandIsValid()
    {
        // arrange
        var ct = new CancellationToken();
        var volunteer = CreateVolunteerWithPets(0);


        var command = new AddPetCommand(
            volunteer.Id,
            "Dog",
            "desc",
            "healthdesc",
            new AddressDto("street", "city", "state", "zip"),
            5,
            5,
            "79494442255",
            DateTime.Now.ToUniversalTime(),
            false,
            false,
            HelpStatusPet.FoundHome,
            new List<RequisiteDto>()
            {
                new RequisiteDto("name", "Desc")
            });

        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<Volunteer>(), ct))
            .ReturnsAsync(Result.Success<Guid, Error>(volunteer.Id.Id));

        var handler = new AddPetHandler(_volunteerRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object);

        // act
        var result = await handler.Execute(command, ct);

        // assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Should().ContainSingle();
    }

    [Fact]
    public async Task Execute_ShouldReturnValidationErrors_WhenCommandIsInvalid()
    {
        // arrange
        var ct = new CancellationToken();
        var volunteer = CreateVolunteerWithPets(0);

        var invalidNumber = "123";

        var command = new AddPetCommand(
            volunteer.Id,
            "Dog",
            "desc",
            "healthdesc",
            new AddressDto("street", "city", "state", "zip"),
            5,
            5,
            invalidNumber,
            DateTime.Now.ToUniversalTime(),
            false,
            false,
            HelpStatusPet.FoundHome,
            new List<RequisiteDto>()
            {
                new RequisiteDto("name", "Desc")
            });
        
        var errorValidate = Errors.General.ValueIsInvalid("PhoneNumber").Serialize();
        var validationFailures = new List<ValidationFailure>
        {
            new("PhoneNumber", errorValidate),
        };
        var validationResult = new ValidationResult(validationFailures);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(validationResult);
        
        var handler = new AddPetHandler(_volunteerRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object);

        // act
        var result = await handler.Execute(command, ct);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().InvalidField.Should().Be("PhoneNumber");
    }

    [Fact]
    public async Task Execute_ShouldReturnSaveError_WhenSavingFails()
    {
        // arrange
        var ct = new CancellationToken();
        var volunteer = CreateVolunteerWithPets(0);
        var command = new AddPetCommand(
            volunteer.Id,
            "Dog",
            "desc",
            "healthdesc",
            new AddressDto("street", "city", "state", "zip"),
            5,
            5,
            "79494442255",
            DateTime.Now.ToUniversalTime(),
            false,
            false,
            HelpStatusPet.FoundHome,
            new List<RequisiteDto>()
            {
                new RequisiteDto("name", "Desc")
            });
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<Volunteer>(), ct))
            .ReturnsAsync(Result.Failure<Guid, Error>(Error.Failure("save.error", "save error")));

        var handler = new AddPetHandler(_volunteerRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object);

        // act
        var result = await handler.Execute(command, ct);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().Code.Should().Be("save.error");
        result.Error.First().Message.Should().Be("save error");
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