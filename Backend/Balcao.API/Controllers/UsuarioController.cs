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
        private readonly IGenericRepository<Anuncio> _anuncioRepository;

        public UsuarioController(IGenericRepository<Usuario> usuarioRepository, IGenericRepository<Anuncio> anuncioRepository, ILogger<UsuarioController> logger)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _anuncioRepository = anuncioRepository;
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
            Usuario usuario = new Usuario();
            usuario.Nome = usuarioDTO.Nome;
            usuario.Senha = Usuario.Criptografar(usuarioDTO.Senha);
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

            if (!usuario.Logar(usuarioDTO.Senha))
            {
                return Unauthorized();
            }

            return Ok(usuario);
        }

        [HttpPost]
        [Route("{id}/criarAnuncio")]
        public IActionResult Create(int id, AnuncioDTO anuncioDTO)
        {
            var usuario = _usuarioRepository.Get(id);
            if (usuario == null)
            {
                return NotFound();
            }
            Anuncio anuncio = new Anuncio();
            anuncio.Proprietario = usuario;
            anuncio.Titulo = anuncioDTO.Titulo;
            anuncio.Descricao = anuncioDTO.Descricao;
            anuncio.Preco = anuncioDTO.Preco;
            if (anuncioDTO.Quantidade != null && anuncioDTO.Quantidade >= 0)
            {
                anuncio.Quantidade = anuncioDTO.Quantidade.Value;
            }
            else
            {
                anuncio.Quantidade = -1;
            }
            //Precisamos receber um boolean do front, perguntando se o tipo do An�ncio � Servi�o ou Produto.
            //Se for Servi�o, set quantidade < 1. Se for Produto, set quantidade = quantidade recebida
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo horaBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            anuncio.DataCriacao = TimeZoneInfo.ConvertTimeFromUtc(dateTime, horaBrasilia);
            _anuncioRepository.Add(anuncio);
            return CreatedAtAction(
                nameof(Get),
                new { id = anuncio.Id },
                anuncio);
        }
    }
}
