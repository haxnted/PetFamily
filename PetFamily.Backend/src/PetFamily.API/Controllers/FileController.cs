using Microsoft.AspNetCore.Mvc;
using Minio;
using PetFamily.API.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.MinIoTest;
using PetFamily.Application.Providers;
namespace PetFamily.API.Controllers;

public class FileController : ApplicationController
{
    private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger)
    {
        _logger = logger;
    }

    [HttpPost("image")]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromServices] UploadFileHandle handle
        )
    {
        await using var stream = file.OpenReadStream();
        var uniqueName = Guid.NewGuid();
        var path = uniqueName + Path.GetExtension(file.FileName);
        
        var fileContent = new FIleContent(stream, path, path);
        var result = await handle.Execute(fileContent);
        
        if (result.IsFailure)
            result.Error.ToResponse();
        
        return Ok(uniqueName);
    }

    [HttpGet("image/{fileName}")]
    public async Task<IActionResult> Unload(
        string fileName,
        [FromServices] UnloadFileHandle handle,
        CancellationToken token)
    {
        var result = await handle.Execute(fileName, token);
        if (result.IsFailure)
            result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpDelete("image/{fileName}")]
    public async Task<IActionResult> Delete(
        string fileName,
        [FromServices] DeleteFileHandle handle,
        CancellationToken token)
    {
        var result = await handle.Execute(fileName, token);
        if (result.IsFailure)
            result.Error.ToResponse();
        
        return Ok(new { Message = "Success" });
    }
}