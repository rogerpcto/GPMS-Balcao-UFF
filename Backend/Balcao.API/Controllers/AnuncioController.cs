﻿using Balcao.API.Services;
using Balcao.Domain.DTOs;
using Balcao.Domain.Entities;
using Balcao.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Balcao.API.Controllers
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

        #region Anúncio

        [HttpGet]
        [AllowAnonymous]
        public IActionResult List(string? consulta, DateTime? dataMinima, DateTime? dataMaxima, float? precoMinimo, float? precoMaximo)
        {
            var anuncios = _anuncioRepository.Query().Where(anuncio => anuncio.Ativo == true);

            if (dataMinima.HasValue)
            {
                anuncios = anuncios.Where(anuncio => anuncio.DataCriacao >= dataMinima);
            }
            if (dataMaxima.HasValue)
            {
                anuncios = anuncios.Where(anuncio => anuncio.DataCriacao <= dataMaxima);
            }
            if (precoMinimo.HasValue)
            {
                anuncios = anuncios.Where(anuncio => anuncio.Preco >= precoMinimo);
            }
            if (precoMaximo.HasValue)
            {
                anuncios = anuncios.Where(anuncio => anuncio.Preco <= precoMaximo);
            }
            if (!string.IsNullOrEmpty(consulta))
            {
                var termos = consulta.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                anuncios = anuncios.Where(anuncio => termos.Any(termo => anuncio.Titulo.ToLower().Contains(termo) || anuncio.Descricao.ToLower().Contains(termo)));
            }

            return Ok(anuncios.ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            return Ok(anuncio);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}/Compras")]
        public IActionResult GetCompras(int id)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Você não tem permissão para ver as compras deste anúncio!");

            return Ok(anuncio.Compras);
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public IActionResult Update(int id, AnuncioDTO anuncioDTO)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Você não tem permissão para alterar este anúncio!");

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

        [HttpPatch]
        [Authorize]
        [Route("{id}/Desativar")]
        public IActionResult Desativar(int id, AnuncioDTO anuncioDTO)
        {
            throw new NotImplementedException();

            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Você não tem permissão para desativar este anúncio!");

            _anuncioRepository.Delete(anuncio);
            return Ok(anuncio);
        }

        #endregion

        #region Compra

        [HttpGet]
        [Authorize]
        [Route("{id}/Compra/{idCompra}")]
        public IActionResult GetCompra(int id, int idCompra)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            Compra? compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(anuncio.Proprietario, User) && !TokenService.EhProprietario(compra.Comprador, User))
                return Unauthorized("Você não tem permissão para acessar esta compra!");

            return Ok(compra);
        }

        [HttpPost]
        [Authorize]
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

            Compra compra = anuncio.IniciarCompra(comprador, quantidade);

            _anuncioRepository.Update(anuncio);

            return CreatedAtAction(
                nameof(GetCompra),
                new { id = anuncio.Id, idCompra = compra.Id },
                compra);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compra/{idCompra}/AguardarPagamento")]
        public IActionResult AguardarPagamento(int id, int idCompra)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (compra.Status != StatusCompra.NEGOCIANDO)
                return BadRequest($"Status da Compra inválida, atualmente é {compra.Status}, deveria ser {StatusCompra.NEGOCIANDO}!");

            if (!TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Somente o proprietário pode mudar o status deste anúncio!");

            compra.AguardarPagamento();

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compra/{idCompra}/EfetuarPagamento")]
        public IActionResult EfetuarPagamento(int id, int idCompra)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (compra.Status != StatusCompra.AGUARDANDO_PAGAMENTO)
                return BadRequest($"Status da Compra inválida, atualmente é {compra.Status}, deveria ser {StatusCompra.AGUARDANDO_PAGAMENTO}!");

            if (!TokenService.EhProprietario(compra.Comprador, User))
                return Unauthorized("Somente o comprador pode mudar o status deste anúncio!");

            compra.EfetuarPagamento();

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compra/{idCompra}/ConfirmarPagamento")]
        public IActionResult ConfirmarPagamento(int id, int idCompra)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (compra.Status != StatusCompra.PAGAMENTO_EFETUADO)
                return BadRequest($"Status da Compra inválida, atualmente é {compra.Status}, deveria ser {StatusCompra.PAGAMENTO_EFETUADO}!");

            if (!TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Somente o proprietário pode mudar o status deste anúncio!");

            compra.ConfirmarPagamento();

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compra/{idCompra}/ConfirmarRecebimento")]
        public IActionResult ConfirmarRecebimento(int id, int idCompra)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (compra.Status != StatusCompra.PAGAMENTO_CONFIRMADO)
                return BadRequest($"Status da Compra inválida, atualmente é {compra.Status}, deveria ser {StatusCompra.PAGAMENTO_CONFIRMADO}!");

            if (!TokenService.EhProprietario(compra.Comprador, User))
                return Unauthorized("Somente o comprador pode mudar o status deste anúncio!");

            compra.ConfirmarRecebimento();

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compra/{idCompra}/AvaliarVendedor")]
        public IActionResult AvaliarVendedor(int id, int idCompra, float nota)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (compra.Status != StatusCompra.PRODUTO_RECEBIDO)
                return BadRequest($"Status da Compra inválida, atualmente é {compra.Status}, deveria ser {StatusCompra.PRODUTO_RECEBIDO}!");

            if (!TokenService.EhProprietario(compra.Comprador, User))
                return Unauthorized("Somente o comprador pode mudar o status deste anúncio!");

            compra.AvaliarVendedor(nota);

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compra/{idCompra}/AvaliarComprador")]
        public IActionResult AvaliarComprador(int id, int idCompra, float nota)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (compra.Status != StatusCompra.VENDEDOR_AVALIADO)
                return BadRequest($"Status da Compra inválida, atualmente é {compra.Status}, deveria ser {StatusCompra.VENDEDOR_AVALIADO}!");

            if (!TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Somente o proprietário pode mudar o status deste anúncio!");

            compra.AvaliarComprador(nota);

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compra/{idCompra}/Concluir")]
        public IActionResult Concluir(int id, int idCompra)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (compra.Status != StatusCompra.COMPRADOR_AVALIADO)
                return BadRequest($"Status da Compra inválida, atualmente é {compra.Status}, deveria ser {StatusCompra.COMPRADOR_AVALIADO}!");

            if (!TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Somente o proprietário pode mudar o status deste anúncio!");

            compra.FecharCompra();

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compra/{idCompra}/Cancelar")]
        public IActionResult Cancelar(int id, int idCompra)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            if (!TokenService.EhProprietario(anuncio.Proprietario, User) && !TokenService.EhProprietario(compra.Comprador, User))
                return Unauthorized("Você não tem permissão para acessar esta compra!");

            compra.CancelarCompra();

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio);
        }

        #endregion
    }
}
