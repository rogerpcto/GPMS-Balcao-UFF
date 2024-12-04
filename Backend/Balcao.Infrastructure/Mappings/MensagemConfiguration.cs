using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Balcao.Domain.Entities;

namespace Balcao.Infrastructure.Mappings
{
    public class MensagemConfiguration : IEntityTypeConfiguration<Mensagem>
    {
        public void Configure(EntityTypeBuilder<Mensagem> builder)
        {
            #region Key

            builder.HasKey(m => m.Id);

            #endregion

            #region Properties

            builder.Property(m => m.TimeStamp)
                .IsRequired();

            builder.Property(m => m.Conteudo)
                .IsRequired()
                .HasMaxLength(1000);

            #endregion
        }
    }
}

