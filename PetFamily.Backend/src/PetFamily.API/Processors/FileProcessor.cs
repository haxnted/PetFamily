using PetFamily.Application.FileProvider;

namespace PetFamily.API.Processors;

public class FileProcessor : IAsyncDisposable
{
    private readonly List<FileContent> _files = [];
    
    public List<FileContent> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileContent = new FileContent(stream, file.FileName);
            _files.Add(fileContent);
        }

        return _files;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _files)
        {
            await file.Stream.DisposeAsync();
        }
    }
}