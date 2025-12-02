using FirearmTracker.Core.Interfaces;
using FirearmTracker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FirearmTracker.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController(
        IDocumentRepository documentRepository,
        FileUploadService fileUploadService) : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly FileUploadService _fileUploadService = fileUploadService;

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            var filePath = _fileUploadService.GetFilePath(document.FileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, document.ContentType, document.OriginalFileName);
        }
    }
}