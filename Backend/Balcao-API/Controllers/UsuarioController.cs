using Balcao.Domain.Entities;
using Balcao.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Balcao_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IGenericRepository<Usuario> _usuarioRepository;

        public UsuarioController(IGenericRepository<Usuario> usuarioRepository, ILogger<UsuarioController> logger)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult List()  
        {
            // Exemplo do uso de LINQ
            //var usuarioLogan = _usuarioRepository.Query().Where(usuario => usuario.Nome == "Logan");
            //var usuarios = _usuarioRepository.Query().ToList();
            var usuarios = new List<Usuario>()
            {
                new Usuario()
                {
                    Nome = "Logan",
                    Senha = "123"
                }
            };
            return Ok(usuarios);
        }
    }
}
