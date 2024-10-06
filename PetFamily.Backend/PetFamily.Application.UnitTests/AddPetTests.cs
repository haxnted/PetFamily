using System.Data;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.Species.Contracts;
using PetFamily.VolunteerManagement.Application;
using PetFamily.VolunteerManagement.Application.Commands.AddPet;
using PetFamily.VolunteerManagement.Domain;
using PetFamily.VolunteerManagement.Domain.Entities;
using PetFamily.VolunteerManagement.Domain.Enums;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.Application.UnitTests;

public class AddPetTests
{
    private readonly Mock<ISpeciesContract> _speciesContractMock = new();
    private readonly Mock<IVolunteerUnitOfWork> _volunteerUnitOfWorkMock = new();
    private readonly Mock<IVolunteersRepository> _volunteerRepositoryMock = new();
    private readonly Mock<IValidator<AddPetCommand>> _validatorMock = new();
    private readonly Mock<ILogger<AddPetHandler>> _loggerMock = new();

    [Fact]
    public async Task Execute_ShouldAddPet_WhenCommandIsValid()
    {
        // arrange
        var ct = new CancellationToken();
        var volunteer = CreateVolunteerWithPets(0);


        var command = new AddPetCommand(volunteer.Id,
            "Dog",
            "desc",
            "healthdesc",
            new AddressDto("street", "city", "state", "zip"),
            5,
            5,
            "79494442255",
            DateTime.Now.ToUniversalTime(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            false,
            HelpStatusPet.FoundHome,
            new List<RequisiteDto> { new("name", "Desc") });

        var speciesDto = CreateSpeciesWithBreeds();

        _volunteerUnitOfWorkMock.Setup(v => v.BeginTransaction(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbTransaction>());

        _speciesContractMock.Setup(s => s.GetSpeciesById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<SpeciesDto, Error>(speciesDto));
        
        _volunteerUnitOfWorkMock.Setup(v => v.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<Volunteer>(), ct))
            .Returns(Task.CompletedTask);
        
        var handler = new AddPetHandler(_volunteerUnitOfWorkMock.Object, _volunteerRepositoryMock.Object,
            _speciesContractMock.Object,
            _validatorMock.Object, _loggerMock.Object);

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

        var command = new AddPetCommand(volunteer.Id,
            "Dog",
            "desc",
            "healthdesc",
            new AddressDto("street", "city", "state", "zip"),
            5,
            5,
            "79494442255",
            DateTime.Now.ToUniversalTime(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            false,
            HelpStatusPet.FoundHome,
            new List<RequisiteDto> { new("name", "Desc") });

        var errorValidate = Errors.General.ValueIsInvalid("PhoneNumber").Serialize();
        var validationFailures = new List<ValidationFailure> { new("PhoneNumber", errorValidate), };
        var validationResult = new ValidationResult(validationFailures);
        var speciesDto = CreateSpeciesWithBreeds();
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(validationResult);

        _volunteerUnitOfWorkMock.Setup(v => v.BeginTransaction(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbTransaction>());

        _speciesContractMock.Setup(s => s.GetSpeciesById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<SpeciesDto, Error>(speciesDto));
        
        var handler = new AddPetHandler(_volunteerUnitOfWorkMock.Object, _volunteerRepositoryMock.Object,
            _speciesContractMock.Object,
            _validatorMock.Object, _loggerMock.Object);

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

        var command = new AddPetCommand(volunteer.Id,
            "Dog",
            "desc",
            "healthdesc",
            new AddressDto("street", "city", "state", "zip"),
            5,
            5,
            "79494442255",
            DateTime.Now.ToUniversalTime(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            false,
            HelpStatusPet.FoundHome,
            new List<RequisiteDto> { new("name", "Desc") });

        var speciesDto = CreateSpeciesWithBreeds();
        
        _speciesContractMock.Setup(s => s.GetSpeciesById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<SpeciesDto, Error>(speciesDto));
        _volunteerUnitOfWorkMock.Setup(v => v.BeginTransaction(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbTransaction>());
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        _volunteerRepositoryMock.Setup(v => v.GetById(It.IsAny<VolunteerId>(), ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<Volunteer>(), ct))
            .Returns(Task.CompletedTask);

        var handler = new AddPetHandler(_volunteerUnitOfWorkMock.Object, _volunteerRepositoryMock.Object,
            _speciesContractMock.Object,
            _validatorMock.Object, _loggerMock.Object);

        // act
        var result = await handler.Execute(command, ct);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().Code.Should().Be("save.error");
        result.Error.First().Message.Should().Be("save error");
    }

    private Volunteer CreateVolunteerWithPets(int petCount)
    {
        var volunteer = new Volunteer(VolunteerId.NewId(),
            FullName.Create("John", "Doe", "sdfsfws").Value,
            Description.Create("General Description").Value,
            AgeExperience.Create(5).Value,
            PhoneNumber.Create("7234567890").Value,
            [],
            []);

        for (int i = 0; i < petCount; i++)
        {
            var pet = new Pet(PetId.NewId(),
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
                []);
            volunteer.AddPet(pet);
        }

        return volunteer;
    }

    private SpeciesDto CreateSpeciesWithBreeds()
    {
        var speciesId = Guid.NewGuid();
        return new SpeciesDto()
        {
            Id = speciesId, Breeds = [new BreedDto { Id = new Guid(), SpeciesId = speciesId, Name = "котэк" }]
        };
    }
}
