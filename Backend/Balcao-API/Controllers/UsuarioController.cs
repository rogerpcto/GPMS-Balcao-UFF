using Balcao.Domain.DTOs;
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
            var usuarios = _usuarioRepository.Query().ToList();
            return Ok(usuarios);
        }

        [HttpPost]
        public IActionResult Create(UsuarioDTO usuarioDTO)
        {
            Usuario usuario = new Usuario();
            usuario.Nome = usuarioDTO.Nome;
            usuario.Senha = usuarioDTO.Senha;
            usuario.Email = usuarioDTO.Email;
            _usuarioRepository.Add(usuario);
            return Created();
        }
    }
}
