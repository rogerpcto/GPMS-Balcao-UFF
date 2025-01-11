namespace Balcao.Domain.DTOs
{
    public class AnuncioDTO
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public float Preco { get; set; }
        public int? Quantidade { get; set; }
        public string Categoria { get; set; }
        public string TipoAnuncio { get; set; }
    }
}
