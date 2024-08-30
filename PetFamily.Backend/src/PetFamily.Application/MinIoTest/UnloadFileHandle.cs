using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.MinIoTest;

public class UnloadFileHandle(IFileProvider fileProvider)
{
    public async Task<Result<string, Error>> Execute(
        string fileName, CancellationToken token = default
    )
    {
        return await fileProvider.Unload(fileName, token);
    }
}