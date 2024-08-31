using CSharpFunctionalExtensions;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.MinIoTest;

public class DeleteFileHandle(IFileProvider fileProvider)
{
    public async Task<Result<string, Error>> Execute(string nameFile, CancellationToken token = default)
    {
        return await fileProvider.Delete(nameFile, token);
    }
}