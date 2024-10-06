using Microsoft.AspNetCore.Http;
using PetFamily.VolunteerManagement.Application.Commands.AddFilesPet;

namespace PetFamily.VolunteerManagement.Presentation.Processors;

public class FileProcessor : IAsyncDisposable
{
    private readonly List<CreateFileCommand> _files = [];
    
    public List<CreateFileCommand> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileContent = new CreateFileCommand(stream, file.FileName);
            _files.Add(fileContent);
        }

        return _files;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _files)
        {
            await file.Content.DisposeAsync();
        }
    }
}