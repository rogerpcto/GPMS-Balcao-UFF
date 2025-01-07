using Balcao.Domain.DTOs;
using Balcao.Domain.Entities;
using Balcao.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Balcao_API.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class AnuncioController : ControllerBase
    {
        private readonly ILogger<AnuncioController> _logger;
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IGenericRepository<Anuncio> _anuncioRepository;

        public AnuncioController(IGenericRepository<Usuario> usuarioRepository, IGenericRepository<Anuncio> anuncioRepository, ILogger<AnuncioController> logger)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _anuncioRepository = anuncioRepository;
        }

        [HttpGet]
        public IActionResult List()
        {
            var anuncios = _anuncioRepository.Query().Where(anuncio => anuncio.Ativo == true).ToList();
            return Ok(anuncios);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound();

            return Ok(anuncio);
        }

        [HttpGet]
        [Route("listarAnunciosUsuario/{idUsuario}")]
        public IActionResult GetAnunciosUsuario(int idUsuario)
        {
            var usuario = _usuarioRepository.Get(idUsuario);
            if (usuario == null)
            {
                return NotFound();
            }
            var anuncios = _anuncioRepository.Query().Where(anuncio => anuncio.Proprietario.Id == idUsuario).ToList();
            return Ok(anuncios);
        }

        [HttpPost]
        public IActionResult Create(AnuncioDTO anuncioDTO)
        {
            Usuario usuario = _usuarioRepository.Get(anuncioDTO.UsuarioId);

            if (usuario == null)
                return NotFound("Usuário não encontrado!");

            var anuncio = new Anuncio();

            anuncio.Proprietario = usuario;

            anuncio.Titulo = anuncioDTO.Titulo;
            anuncio.Descricao = anuncioDTO.Descricao;
            anuncio.Preco = anuncioDTO.Preco;
            if (anuncioDTO.Quantidade.HasValue)
                anuncio.Quantidade = anuncioDTO.Quantidade.Value;

            anuncio.Ativo = true;
            anuncio.DataCriacao = DateTime.Now;

            _anuncioRepository.Add(anuncio);

            return CreatedAtAction(
                nameof(Get),
                new { id = anuncio.Id },
                anuncio);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, AnuncioDTO anuncioDTO)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound();

            var usuario = _usuarioRepository.Get(anuncioDTO.UsuarioId);
            if (usuario == null)
            {
                return Unauthorized();
            }
            if (usuario.Id != anuncio.Proprietario.Id && usuario.Perfil == Perfil.USUARIO)
            {
                return Unauthorized();
            }

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
            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id, AnuncioDTO anuncioDTO)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound();

            var usuario = _usuarioRepository.Get(anuncioDTO.UsuarioId);
            if (usuario == null)
            {
                return Unauthorized();
            }
            if (usuario.Id != anuncio.Proprietario.Id && usuario.Perfil == Perfil.USUARIO) //adicionar condição tambem se usuario nao for do Perfil admin
            {
                return Unauthorized();
            }
            _anuncioRepository.Delete(anuncio);
            return Ok(anuncio);
        }

        [HttpGet]
        [Route("{id}/Compra/{idCompra}")]
        public IActionResult GetCompra(int id, int idCompra)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            Compra? compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound();

            return Ok(compra);
        }

        [HttpPost]
        [Route("{id}/Compra")]
        public IActionResult CreateCompra(int id, int idComprador, int quantidade)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            Usuario comprador = _usuarioRepository.Get(idComprador);

            if (comprador == null)
                return NotFound("Usuário do comprador da compra não encontrado!");

            if (comprador == anuncio.Proprietario)
                return BadRequest("Usuário comprador não pode ser o proprietário do anúncio!");

            var compra = new Compra();

            compra.Autor = anuncio.Proprietario;
            compra.Comprador = comprador;
            compra.Quantidade = quantidade;
            anuncio.Compras.Add(compra);

            _anuncioRepository.Update(anuncio);

            return CreatedAtAction(
                nameof(GetCompra),
                new { id = anuncio.Id, idCompra = compra.Id },
                compra);
        }

        [HttpPatch]
        [Route("{id}/Compra/{idCompra}/AvaliarVendedor")]
        public IActionResult AvaliarVendedor(int id, int idCompra, float nota)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound();

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound();

            if (compra.Status != StatusCompra.CONCLUIDO)
                return BadRequest("Não é possível avaliar um vendedor sem conculir a compra!");

            compra.AvaliarVendedor(nota);

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }
    }
}
