using Balcao.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Balcao.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Anuncio> Anuncios { get; set; }
        public DbSet<Compra> Compras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                Id = 1,
                Nome = "Administrador",
                Email = "admin@test.com",
                Senha = Usuario.Criptografar("Admin@123"),
                Nota = 0,
                Perfil = Perfil.ADMINISTRADOR
            });
        }
    }
}
