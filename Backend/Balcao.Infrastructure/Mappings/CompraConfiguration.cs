using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Balcao.Domain.Entities;

namespace Balcao.Infrastructure.Mappings
{
    public class CompraConfiguration : IEntityTypeConfiguration<Compra>
    {
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            #region Key

            builder.HasKey(c => c.Id);

            #endregion

            #region Properties

            builder.Property(c => c.Status)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (StatusCompra)Enum.Parse(typeof(StatusCompra), v))
                .HasMaxLength(50);

            #endregion

            #region Relationship

            builder.HasOne<Anuncio>()
                .WithMany(a => a.Compras)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Usuario>()
                .WithMany(u => u.Compras)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }
    }
}

