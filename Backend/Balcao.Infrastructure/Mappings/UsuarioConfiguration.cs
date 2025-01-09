using Balcao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balcao.Infrastructure.Mappings
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            #region Key

            builder.HasKey(u => u.Id);

            #endregion

            #region Properties

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Senha)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Nota)
                .IsRequired();

            builder.Property(u => u.Perfil)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (Perfil)Enum.Parse(typeof(Perfil), v)
                );

            #endregion

            #region Relationships

            builder.HasMany(u => u.Compras)
                .WithOne(c => c.Comprador)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }
    }

}
