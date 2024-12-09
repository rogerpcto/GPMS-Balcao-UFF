using Balcao.Domain.DTOs;
using Balcao.Domain.Entities;
using Balcao.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

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
                return NotFound();
            }
            if (usuario.Id != anuncio.Proprietario.Id && usuario.Perfil == Perfil.USUARIO) 
            {
                return Unauthorized();
            }
            
            anuncio.Titulo = anuncioDTO.Titulo;
            anuncio.Descricao = anuncioDTO.Descricao;
            anuncio.Preco = anuncioDTO.Preco;
            anuncio.Quantidade = anuncioDTO.Quantidade;
            anuncio.Ativo = anuncioDTO.Ativo;
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
                return NotFound();
            }
            if (usuario.Id != anuncio.Proprietario.Id && usuario.Perfil == Perfil.USUARIO) //adicionar condição tambem se usuario nao for do Perfil admin
            {
                return Unauthorized();
            }
            _anuncioRepository.Delete(anuncio);
            return Ok(anuncio);
        }
    }
}
