using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<string, Error>> Upload(FIleContent content, CancellationToken token = default);
    Task<Result<string, Error>> Unload(string fileName, CancellationToken token = default);
    Task<Result<string, Error>> Delete(string fileName, CancellationToken token = default);
}