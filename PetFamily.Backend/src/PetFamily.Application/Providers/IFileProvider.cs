using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    Task<Result<IEnumerable<string>, Error>> UploadFiles(IEnumerable<FileContent> content, CancellationToken token = default);
    Task<Result<string, Error>> GetFileByName(string fileName, CancellationToken token = default);
    Task<Result<string, Error>> Delete(string fileName, CancellationToken token = default);
}