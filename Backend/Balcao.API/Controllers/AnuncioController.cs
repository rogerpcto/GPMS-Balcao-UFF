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

            return Ok(anuncios.ToList().Select(u => u.ToJson()).ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            return Ok(anuncio.ToJson());
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

            return Ok(anuncio.Compras.Select(c => c.ToJson()));
        }


        [HttpPost]
        [Authorize]
        public IActionResult Create(AnuncioDTO anuncioDTO)
        {
            int idUsuario = TokenService.GetIdUsuario(User);
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
            anuncio.Localizacao = anuncioDTO.Localizacao;
            anuncio.Contato = anuncioDTO.Contato;
            anuncio.Categoria = Enum.TryParse<Categoria>(anuncioDTO.Categoria, out var categoria) ? categoria : Categoria.OUTROS;
            Enum.TryParse<TipoAnuncio>(anuncioDTO.TipoAnuncio, out var tipoAnuncio);
            anuncio.TipoAnuncio = tipoAnuncio;

            anuncio.Ativo = true;
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo horaBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            anuncio.DataCriacao = TimeZoneInfo.ConvertTimeFromUtc(dateTime, horaBrasilia);

            _anuncioRepository.Add(anuncio);

            return CreatedAtAction(
                nameof(Get),
                new { id = anuncio.Id },
                anuncio.ToJson());
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
            anuncio.Localizacao = anuncioDTO.Localizacao;
            anuncio.Contato = anuncioDTO.Contato;
            anuncio.Categoria = Enum.TryParse<Categoria>(anuncioDTO.Categoria, out var categoria) ? categoria : Categoria.OUTROS;
            Enum.TryParse<TipoAnuncio>(anuncioDTO.TipoAnuncio, out var tipoAnuncio);
            anuncio.TipoAnuncio = tipoAnuncio;

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Desativar")]
        public IActionResult Desativar(int id)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Você não tem permissão para desativar este anúncio!");

            anuncio.Desativar();

            _anuncioRepository.Update(anuncio);
            return Ok(anuncio.ToJson());
        }

        #endregion

        #region Compra

        [HttpGet]
        [Authorize]
        [Route("{id}/Compras/{idCompra}")]
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

            return Ok(compra.ToJson());
        }

        [HttpGet]
        [Authorize]
        [Route("ListarCompras")]
        public IActionResult GetCompras()
        {
            int idUsuario = TokenService.GetIdUsuario(User);
            var usuario = _usuarioRepository.Get(idUsuario);
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado!");
            }

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(usuario, User))
                return Unauthorized("Você não tem permissão para ver as compras deste usuário!");

            return Ok(usuario.Compras.Select(c => c.ToJson()));
        }

        [HttpPost]
        [Authorize]
        [Route("{id}/Compras")]
        public IActionResult CreateCompra(int id, int quantidade)
        {
            int idComprador = TokenService.GetIdUsuario(User);

            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            if (!anuncio.Ativo)
                return BadRequest("Anúncio não está mais ativo!");

            if (anuncio.Quantidade < quantidade)
                return BadRequest("Quantidade solicitada maior que a quantidade disponível!");

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
                compra.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/AguardarPagamento")]
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
            return Ok(anuncio.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/EfetuarPagamento")]
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
            return Ok(anuncio.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/ConfirmarPagamento")]
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
            return Ok(anuncio.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/ConfirmarRecebimento")]
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
            return Ok(anuncio.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/AvaliarVendedor")]
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
            return Ok(anuncio.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/AvaliarComprador")]
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
            return Ok(anuncio.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/Concluir")]
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
            return Ok(anuncio.ToJson());
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/Cancelar")]
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
            return Ok(anuncio.ToJson());
        }

        [HttpPost]
        [Authorize]
        [Route("{id}/Compras/{idCompra}/Mensagens")]
        public IActionResult Mensagem(int id, int idCompra, string texto)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            var compra = anuncio.Compras.FirstOrDefault(c => c.Id == idCompra);

            if (compra == null)
                return NotFound("Compra não encontrada!");

            bool proprietario = TokenService.EhProprietario(anuncio.Proprietario, User);

            if (!proprietario && !TokenService.EhProprietario(compra.Comprador, User))
                return Unauthorized("Você não tem permissão para mandar mensagem nesta compra!");

            var mensagem = new Mensagem
            {
                Conteudo = texto,
                TimeStamp = DateTime.Now,
                Proprietario = proprietario
            };

            compra.Mensagens.Add(mensagem);
            _anuncioRepository.Update(anuncio);

            return Ok(mensagem.ToJson());
        }

        #endregion

        #region Imagens

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}/Imagens")]
        public IActionResult GetImagens(int id)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            return Ok(anuncio.Imagem);
        }

        [HttpPost]
        [Authorize]
        [Route("{id}/Imagens")]
        public async Task<IActionResult> UploadImagem(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Arquivo inválido!");
            }

            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Você não tem permissão para alterar este anúncio!");

            string nomeArquivo = $"{id}_{anuncio.Imagem.Count + 1}" + Path.GetExtension(file.FileName);

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Imagens");
            var filePath = Path.Combine(directoryPath, nomeArquivo);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            Imagem imagem = new Imagem();
            imagem.Url = nomeArquivo;

            anuncio.Imagem.Add(imagem);

            _anuncioRepository.Update(anuncio);

            var imageUrl = Url.Action("Get", "Imagem", new { fileName = imagem.Url });

            return Created(imageUrl, imagem);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}/Imagens")]
        public async Task<IActionResult> DeleteImagem(int id, string fileName)
        {
            var anuncio = _anuncioRepository.Get(id);

            if (anuncio == null)
                return NotFound("Anúncio não encontrado!");

            if (!TokenService.EhAdmin(User) && !TokenService.EhProprietario(anuncio.Proprietario, User))
                return Unauthorized("Você não tem permissão para alterar este anúncio!");

            var imagem = anuncio.Imagem.FirstOrDefault(i => i.Url == fileName);

            if (imagem == null)
                return NotFound("Imagem não encontrada!");

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Imagens");
            var filePath = Path.Combine(directoryPath, imagem.Url);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else
            {
                return NotFound("Imagem não encontrada no servidor!");
            }

            anuncio.Imagem.Remove(imagem);

            _anuncioRepository.Update(anuncio);

            return Ok("Removido com sucesso!");
        }

        #endregion
    }
}
