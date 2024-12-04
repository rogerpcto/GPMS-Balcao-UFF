using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Balcao.Domain.Entities;

namespace Balcao.Infrastructure.Mappings
{
    public class ImagemConfiguration : IEntityTypeConfiguration<Imagem>
    {
        public void Configure(EntityTypeBuilder<Imagem> builder)
        {
            #region Key

            builder.HasKey(i => i.Id);

            #endregion

            #region Properties

            builder.Property(i => i.Url)
                .IsRequired()
                .HasMaxLength(500);

            #endregion

            #region Relationships

            builder.HasOne<Anuncio>()
                .WithMany(a => a.Imagem)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}

