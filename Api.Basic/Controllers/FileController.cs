using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Api.Basic.Controllers;

//[Route("api/[controller]")]
[Route("api/files")]  // recommended this way!

[ApiController]
public class FileController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;


    public FileController(FileExtensionContentTypeProvider contentTypeProvider)
    {
        _contentTypeProvider = contentTypeProvider ?? 
                               throw new ArgumentException(nameof(contentTypeProvider));
    }




    [HttpGet("{fileId}")]
    public ActionResult GetFile(int fileId)
    {
        // look up the actual file, depending on file id!

        var pathToFile = "Test-For-Download.pdf";



        // check if file exist

        if (!System.IO.File.Exists(pathToFile))
        {
            return NotFound();
        }


        if (!_contentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            contentType = "application/octet-stream";


        var bytes = System.IO.File.ReadAllBytes(pathToFile);
        return File(bytes, contentType, Path.GetFileName(pathToFile));



    }

    // don't forget to see r click on pdf file on solution explorer!!!
}

