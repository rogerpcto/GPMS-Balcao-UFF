namespace Balcao.Domain.Entities
{
    public class Mensagem
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Conteudo { get; set; }
        public bool Proprietario { get; set; }

        public object? ToJson()
        {
            return new
            {
                Id,
                TimeStamp,
                Conteudo,
                Proprietario
            };
        }
    }
}
