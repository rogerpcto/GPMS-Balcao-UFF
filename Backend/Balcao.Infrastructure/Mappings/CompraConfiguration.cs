using Balcao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder.Property(c => c.Nota)
                .IsRequired();

            builder.Property(c => c.Quantidade)
                .IsRequired();

            builder.Property(c => c.Status)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (StatusCompra)Enum.Parse(typeof(StatusCompra), v))
                .HasMaxLength(50);

            #endregion
        }
    }
}

