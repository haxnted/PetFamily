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
    private readonly Mock<IVolunteersRepository> _volunteerRepositoryMock;
    private readonly Mock<IValidator<AddPhotosToPetCommand>> _validatorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<AddPhotosToPetHandler>> _loggerMock;
    private readonly Mock<IFileProvider> _fileProviderMock;
    private readonly Mock<IDbTransaction> _dbTransactionMock;
    public AddPhotosToPetTest()
    {
        _volunteerRepositoryMock = new Mock<IVolunteersRepository>();
        _validatorMock = new Mock<IValidator<AddPhotosToPetCommand>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<AddPhotosToPetHandler>>();
        _fileProviderMock = new Mock<IFileProvider>();
        _dbTransactionMock = new Mock<IDbTransaction>();
    }
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
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());

        _volunteerRepositoryMock.Setup(v => v.Save(It.IsAny<Volunteer>(), ct))
            .ReturnsAsync(Result.Success<Guid, Error>(volunteerId.Id));

        _volunteerRepositoryMock.Setup(v => v.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));
        
        _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.BeginTransaction(ct))
            .ReturnsAsync(_dbTransactionMock.Object);

        var returnedFilePaths = fileContents
            .Select(f => f.File)
            .Select(f => f.Path).ToList();

        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<List<FileContent>>(), ct))
            .ReturnsAsync(Result.Success<IEnumerable<string>, Error>(returnedFilePaths));
        
        var handler = new AddPhotosToPetHandler(
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _volunteerRepositoryMock.Object,
            _fileProviderMock.Object,
            _loggerMock.Object);

        // act
        var resultHandle = await handler.Execute(command, ct);

        // assert
        resultHandle.IsSuccess.Should().BeTrue();
        resultHandle.Value.Equals(volunteerId.Id).Should().BeTrue();
    }
    
    [Fact]
    public async Task Execute_With_Invalid_Command_Should_Return_Validation_Errors()
    {
        // arrange
        var ct = new CancellationToken();

        var volunteer = CreateVolunteerWithPets(1);
        var volunteerId = volunteer.Id;
        var petId = volunteer.Pets[0].Id;

        var command = new AddPhotosToPetCommand(volunteerId, petId, CreateAddPhotosToPetCommand());

        var errorValidate = Errors.General.ValueIsInvalid(nameof(command.Files)).Serialize();
        var validationFailures = new List<ValidationFailure>
        {
            new (nameof(command.Files), errorValidate),
        };
        var validationResult = new ValidationResult(validationFailures);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(validationResult);
        
        var handler = new AddPhotosToPetHandler(
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _volunteerRepositoryMock.Object,
            _fileProviderMock.Object,
            _loggerMock.Object
        );

        // act
        var result = await handler.Execute(command, ct);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.First().InvalidField.Should().Be(nameof(command.Files));
    }

    [Fact]
    public async Task Execute_Volunteer_Not_Found_Should_Return_Not_Found_Error()
    {
        // arrange
        var ct = new CancellationToken();

        var volunteerId = VolunteerId.NewId();
        var petId = PetId.NewId();

        var command = new AddPhotosToPetCommand(volunteerId, petId, CreateAddPhotosToPetCommand());
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        _volunteerRepositoryMock.Setup(v => v.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Failure<Volunteer, Error>(Errors.General.NotFound(volunteerId.Id)));

        var handler = new AddPhotosToPetHandler(
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _volunteerRepositoryMock.Object,
            _fileProviderMock.Object,
            _loggerMock.Object
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
        
        _validatorMock.Setup(v => v.ValidateAsync(command, ct))
            .ReturnsAsync(new ValidationResult());
        
        _volunteerRepositoryMock.Setup(v => v.GetById(volunteerId, ct))
            .ReturnsAsync(Result.Success<Volunteer, Error>(volunteer));
        
        _unitOfWorkMock.Setup(u => u.BeginTransaction(ct))
            .ReturnsAsync(_dbTransactionMock.Object);
        
        _fileProviderMock.Setup(f => f.UploadFiles(It.IsAny<List<FileContent>>(), ct))
            .ReturnsAsync(Result.Failure<IEnumerable<string>, Error>(
                Error.Failure("failed.add.photos", "Failed add photos to pet"))
            );

        var loggerMock = new Mock<ILogger<AddPhotosToPetHandler>>();

        var handler = new AddPhotosToPetHandler(
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _volunteerRepositoryMock.Object,
            _fileProviderMock.Object,
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