using PetFamily.Application.Features.Volunteers.AddFilesPet;

namespace PetFamily.API.Processors;

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