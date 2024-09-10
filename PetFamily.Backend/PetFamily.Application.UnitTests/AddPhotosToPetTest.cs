using System.Data;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Volunteers;
using PetFamily.Application.Features.Volunteers.AddFilesPet;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.UnitTests;

public class AddPhotosToPetTest
{
    [Fact]
    public async void Execute_Should_Upload_Files_To_Pet()
    {
        // arrange
        var ct = new CancellationToken();

        var volunteer = CreateVolunteerWithPets(1);
        var volunteerId = volunteer.Id;
        var petId = volunteer.Pets[0].Id;

        var fileContents = new List<FileContent>
        {
            new (new MemoryStream(), FilePath.Create(Guid.NewGuid() + ".png").Value, "files"),
            new (new MemoryStream(), FilePath.Create(Guid.NewGuid() + ".png").Value, "files")
        };
        var createFileCommand = fileContents.Select(f => new CreateFileCommand(f.Stream, f.File.Path));
        var command = new AddPhotosToPetCommand(volunteerId, petId, createFileCommand);

        var validatorMock = new Mock<IValidator<AddPhotosToPetCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        var volunteerRepositoryMock = new Mock<IVolunteersRepository>();
        volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<Volunteer>(), ct))
            .ReturnsAsync(Result.Success<Guid, Error>(volunteerId.Id));

        volunteerRepositoryMock.Setup(v => v.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        var dbTransactionMock = new Mock<IDbTransaction>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.BeginTransaction(ct))
            .ReturnsAsync(dbTransactionMock.Object);

        var returnedFilePaths = fileContents
            .Select(f => f.File)
            .Select(f => f.Path).ToList();
        var fileProviderMock = new Mock<IFileProvider>();
        fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<List<FileContent>>(), ct))
            .ReturnsAsync(Result.Success<IEnumerable<string>, Error>(returnedFilePaths));

        var loggerMock = new Mock<ILogger<AddPhotosToPetHandler>>();

        var handler = new AddPhotosToPetHandler(
            unitOfWorkMock.Object,
            validatorMock.Object,
            volunteerRepositoryMock.Object,
            fileProviderMock.Object,
            loggerMock.Object);

        // act
        var resultHandle = await handler.Execute(command, ct);

        // assert
        resultHandle.IsSuccess.Should().BeTrue();
        resultHandle.Value.Equals(volunteerId.Id).Should().BeTrue();
    }

    public async Task Execute_With_Invalid_Command_Should_Return_Validation_Errors()
    {
        // arrange
        var ct = new CancellationToken();

        var volunteer = CreateVolunteerWithPets(1);
        var volunteerId = volunteer.Id;
        var petId = volunteer.Pets[0].Id;

        var command = new AddPhotosToPetCommand(volunteerId, petId, CreateAddPhotosToPetCommand());
        
        var validationFailures = new List<ValidationFailure>
        {
            new ("VolunteerId", "Invalid Volunteer ID"),
            new ("PetId", "Invalid Pet ID")
        };
        var validationResult = new ValidationResult(validationFailures);

        var validatorMock = new Mock<IValidator<AddPhotosToPetCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(validationResult);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var volunteerRepositoryMock = new Mock<IVolunteersRepository>();
        var fileProviderMock = new Mock<IFileProvider>();
        var loggerMock = new Mock<ILogger<AddPhotosToPetHandler>>();

        var handler = new AddPhotosToPetHandler(
            unitOfWorkMock.Object,
            validatorMock.Object,
            volunteerRepositoryMock.Object,
            fileProviderMock.Object,
            loggerMock.Object
        );

        // act
        var result = await handler.Execute(command, ct);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().Should().Be("Invalid Volunteer ID");
        result.Error.Last().Should().Be("Invalid Pet ID");
    }

    [Fact]
    public async Task Execute_Volunteer_Not_Found_Should_Return_Not_Found_Error()
    {
        // arrange
        var ct = new CancellationToken();

        var volunteerId = VolunteerId.NewId();
        var petId = PetId.NewId();

        var command = new AddPhotosToPetCommand(volunteerId, petId, CreateAddPhotosToPetCommand());
        
        var validatorMock = new Mock<IValidator<AddPhotosToPetCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        var volunteerRepositoryMock = new Mock<IVolunteersRepository>();
        volunteerRepositoryMock.Setup(v => v.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Failure<Volunteer, Error>(Errors.General.NotFound(volunteerId.Id)));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var fileProviderMock = new Mock<IFileProvider>();
        var loggerMock = new Mock<ILogger<AddPhotosToPetHandler>>();

        var handler = new AddPhotosToPetHandler(
            unitOfWorkMock.Object,
            validatorMock.Object,
            volunteerRepositoryMock.Object,
            fileProviderMock.Object,
            loggerMock.Object
        );

        // act
        var result = await handler.Execute(command, ct);

        // assert
        result.IsFailure.Should().BeTrue();
        var error = result.Error.First();
        error.Code.Should().Be("record.not.found");
        error.Message.Should().Contain("record not found for Id");
        error.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Execute_FileUploadFails_Should_Return_File_Upload_Error()
    {
        // arrange
        var ct = new CancellationToken();

        var volunteer = CreateVolunteerWithPets(1);
        var volunteerId = volunteer.Id;
        var petId = volunteer.Pets[0].Id;
        
        var command = new AddPhotosToPetCommand(volunteerId, petId, CreateAddPhotosToPetCommand());

        var validatorMock = new Mock<IValidator<AddPhotosToPetCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        var volunteerRepositoryMock = new Mock<IVolunteersRepository>();
        volunteerRepositoryMock.Setup(v => v.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var dbTransactionMock = new Mock<IDbTransaction>();
        unitOfWorkMock.Setup(u => u.BeginTransaction(ct))
            .ReturnsAsync(dbTransactionMock.Object);
        
        var fileProviderMock = new Mock<IFileProvider>();
        fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<List<FileContent>>(), ct))
            .ReturnsAsync(Result.Failure<IEnumerable<string>, Error>(
                Error.Failure("failed.add.photos", "Failed add photos to pet"))
            );

        var loggerMock = new Mock<ILogger<AddPhotosToPetHandler>>();

        var handler = new AddPhotosToPetHandler(
            unitOfWorkMock.Object,
            validatorMock.Object,
            volunteerRepositoryMock.Object,
            fileProviderMock.Object,
            loggerMock.Object
        );

        // act
        var result = await handler.Execute(command, ct);
        // assert
        result.IsFailure.Should().BeTrue();
        var error = result.Error.First();
        error.Code.Should().Be("failed.add.photos");
        error.Message.Should().Contain("Failed add photos to pet");
        error.Type.Should().Be(ErrorType.Failure);
    }

    private IEnumerable<CreateFileCommand> CreateAddPhotosToPetCommand()
    {
        var fileContents = new List<FileContent>
        {
            new (new MemoryStream(), FilePath.Create(Guid.NewGuid() + ".png").Value, "files"),
            new (new MemoryStream(), FilePath.Create(Guid.NewGuid() + ".png").Value, "files")
        };
        return fileContents.Select(f => new CreateFileCommand(f.Stream, f.File.Path));
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