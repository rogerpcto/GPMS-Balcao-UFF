﻿using Balcao.Domain.DTOs;
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
                return Unauthorized();
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
