using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.MinIoTest;

public class UploadFileHandle(IFileProvider fileProvider)
{
    public async Task<Result<string, Error>> Execute(
        FIleContent request, CancellationToken token = default
    )
    {
        return await fileProvider.Upload(request, token);
    }
}