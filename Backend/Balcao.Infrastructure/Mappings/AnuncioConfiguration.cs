using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Balcao.Domain.Entities;

namespace Balcao.Infrastructure.Mappings
{
    public class AnuncioConfiguration : IEntityTypeConfiguration<Anuncio>
    {
        public void Configure(EntityTypeBuilder<Anuncio> builder)
        {
            #region Key

            builder.HasKey(a => a.Id);

            #endregion

            # region Properties

            builder.Property(a => a.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Descricao)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.Ativo)
                .IsRequired();

            builder.Property(a => a.Nota)
                .IsRequired();

            builder.Property(a => a.DataCriacao)
                .IsRequired();

            builder.Property(a => a.Preco)
                .IsRequired();

            builder.Property(a => a.Quantidade)
                .IsRequired();

            #endregion

            #region Relationships

            builder.HasOne(a => a.Proprietario)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(a => a.Imagem)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Compras)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }
    }
}

