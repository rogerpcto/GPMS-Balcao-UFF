using Balcao.API.Services;
using Balcao.Domain.DTOs;
using Balcao.Domain.Entities;
using Balcao.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Balcao.API.Controllers
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
        [AllowAnonymous]
        public IActionResult List()
        {
            var usuarios = _usuarioRepository.Query().ToList();
            return Ok(usuarios);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _usuarioRepository.Get(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado!");

            return Ok(usuario);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create(UsuarioDTO usuarioDTO)
        {
            if (string.IsNullOrEmpty(usuarioDTO.Nome) || string.IsNullOrEmpty(usuarioDTO.Email) || string.IsNullOrEmpty(usuarioDTO.Senha))
            {
                return BadRequest("Nome, e-mail e senha são obrigatórios!");
            }

            if (_usuarioRepository.Query().Any(u => u.Email.ToLower() == usuarioDTO.Email.ToLower()))
                return BadRequest("Já existe um usuário com esse e-mail!");

            Usuario usuario = new Usuario();
            usuario.Nome = usuarioDTO.Nome;
            usuario.Senha = Usuario.Criptografar(usuarioDTO.Senha);
            usuario.Email = usuarioDTO.Email.ToLower();
            usuario.Perfil = Perfil.USUARIO;
            _usuarioRepository.Add(usuario);
            return CreatedAtAction(
                nameof(Get),
                new { id = usuario.Id },
                usuario);
        }

        [HttpPost]
        [Authorize]
        [Route("CriarAdmin")]
        public IActionResult CriarAdmin(UsuarioDTO usuarioDTO)
        {
            if (!TokenService.EhAdmin(User) && _usuarioRepository.Query().Any(u => u.Perfil == Perfil.ADMINISTRADOR))
                return Unauthorized("Apenas administradores podem criar usuários administradores!");

            if (string.IsNullOrEmpty(usuarioDTO.Nome) || string.IsNullOrEmpty(usuarioDTO.Email) || string.IsNullOrEmpty(usuarioDTO.Senha))
            {
                return BadRequest("Nome, e-mail e senha são obrigatórios!");
            }

            Usuario usuario = new Usuario();
            usuario.Nome = usuarioDTO.Nome;
            usuario.Senha = Usuario.Criptografar(usuarioDTO.Senha);
            usuario.Email = usuarioDTO.Email.ToLower();
            usuario.Perfil = Perfil.ADMINISTRADOR;
            _usuarioRepository.Add(usuario);
            return CreatedAtAction(
                nameof(Get),
                new { id = usuario.Id },
                usuario);
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public IActionResult Update(int id, UsuarioDTO usuarioDTO)
        {
            var usuario = _usuarioRepository.Get(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(usuario, User))
                return Unauthorized("Você não tem permissão para alterar este usuário!");

            if (string.IsNullOrEmpty(usuarioDTO.Nome))
                usuario.Nome = usuarioDTO.Nome;

            if (string.IsNullOrEmpty(usuarioDTO.Senha))
                usuario.Senha = usuarioDTO.Senha;

            if (string.IsNullOrEmpty(usuarioDTO.Email))
                usuario.Email = usuarioDTO.Email.ToLower();

            _usuarioRepository.Update(usuario);
            return Ok(usuario);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var usuario = _usuarioRepository.Get(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(usuario, User))
                return Unauthorized("Você não tem permissão para apagar este usuário!");

            try
            {
                _usuarioRepository.Delete(usuario);
            }
            catch
            {
                return BadRequest("Usuário não pode ser apagado por ter participado em anúncios.");
            }

            return Ok(usuario);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _usuarioRepository.Query().FirstOrDefault(x => x.Email == email);

            if (usuario == null || !usuario.Logar(senha))
            {
                return Unauthorized("Login ou senha incorretos!");
            }

            var token = TokenService.GenerateToken(usuario);

            return Ok(new { message = "Login bem-sucedido", token });
        }

        [HttpPost]
        [Authorize]
        [Route("{idUsuario}/CriarAnuncio")]
        public IActionResult Create(int idUsuario, AnuncioDTO anuncioDTO)
        {
            var usuario = _usuarioRepository.Get(idUsuario);
            if (usuario == null)
                return NotFound("Usuário não encontrado!");

            if (!TokenService.EhProprietario(usuario, User))
                return Unauthorized("Você não tem permissão para criar anúncios para esse usuário!");

            Anuncio anuncio = new Anuncio();
            anuncio.Proprietario = usuario;
            anuncio.Titulo = anuncioDTO.Titulo;
            anuncio.Descricao = anuncioDTO.Descricao;
            anuncio.Preco = anuncioDTO.Preco;
            if (anuncioDTO.Quantidade.HasValue && anuncioDTO.Quantidade >= 0)
            {
                anuncio.Quantidade = anuncioDTO.Quantidade.Value;
            }
            else
            {
                anuncio.Quantidade = -1;
            }

            anuncio.Ativo = true;
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo horaBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            anuncio.DataCriacao = TimeZoneInfo.ConvertTimeFromUtc(dateTime, horaBrasilia);

            _anuncioRepository.Add(anuncio);

            return CreatedAtAction(
                nameof(Get),
                new { id = anuncio.Id },
                anuncio);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{idUsuario}/ListarAnuncios")]
        public IActionResult GetAnuncios(int idUsuario)
        {
            var usuario = _usuarioRepository.Get(idUsuario);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado!");
            }
            var anuncios = _anuncioRepository.Query().Where(anuncio => anuncio.Proprietario.Id == idUsuario).ToList();
            return Ok(anuncios);
        }

        [HttpGet]
        [Authorize]
        [Route("{idUsuario}/ListarCompras")]
        public IActionResult GetCompras(int idUsuario)
        {
            var usuario = _usuarioRepository.Get(idUsuario);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado!");
            }

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(usuario, User))
                return Unauthorized("Você não tem permissão para ver as compras deste usuário!");

            return Ok(usuario.Compras);
        }
    }
}
