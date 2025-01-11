using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Balcao.API.Controllers
{
    [ApiController]
    [Route("Imagens")]
    public class ImagemController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;

        public ImagemController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get(string fileName)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Imagens");
            var filePath = Path.Combine(directoryPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Imagem não encontrada.");
            }

            var mimeType = GetMimeType(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, mimeType);
        }

        private static string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }
    }
}
