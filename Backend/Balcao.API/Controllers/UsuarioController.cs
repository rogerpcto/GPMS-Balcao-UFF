using Balcao.Domain.DTOs;
using Balcao.Domain.Entities;
using Balcao.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Balcao_API.Controllers
{
    [ApiController]
    [Route("[controller]s")]
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

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _usuarioRepository.Get(id);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Create(UsuarioDTO usuarioDTO)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(10);

            Usuario usuario = new Usuario();
            usuario.Nome = usuarioDTO.Nome;
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Senha, salt);
            usuario.Email = usuarioDTO.Email;
            _usuarioRepository.Add(usuario);
            return CreatedAtAction(
                nameof(Get),
                new { id = usuario.Id },
                usuario);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, UsuarioDTO usuarioDTO)
        {
            var usuario = _usuarioRepository.Get(id);

            if (usuario == null)

                return NotFound();

            usuario.Nome = usuarioDTO.Nome;
            usuario.Senha = usuarioDTO.Senha;
            usuario.Email = usuarioDTO.Email;
            _usuarioRepository.Update(usuario);
            return Ok(usuario);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var usuario = _usuarioRepository.Get(id);

            if (usuario == null)
                return NotFound();

            _usuarioRepository.Delete(usuario);
            return Ok(usuario);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
            {
                return BadRequest();
            }

            var usuario = _usuarioRepository.Query().FirstOrDefault(x => x.Email == usuarioDTO.Email);
            if (usuario == null)
            {
                return NotFound();
            }

            if (!usuario.Logar(usuarioDTO.Senha, usuario.Senha))
            {
                return Unauthorized();
            }

            return Ok(usuario);
        }
    }
}
