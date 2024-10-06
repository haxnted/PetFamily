using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.Core.Providers.FileProvider;

public interface IFileProvider
{
    Task<Result<IEnumerable<string>, Error>> UploadFiles(
        IEnumerable<FileContent> content, CancellationToken token = default);

    Task<Result<string, Error>> GetFileByName(string fileName, string bucketName, CancellationToken token = default);
    Task<Result<string, Error>> Delete(string fileName, string bucketName, CancellationToken token = default);
}
