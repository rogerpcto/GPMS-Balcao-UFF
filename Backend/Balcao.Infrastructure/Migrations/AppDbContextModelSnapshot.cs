﻿// <auto-generated />
using System;
using Balcao.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Balcao.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Balcao.Domain.Entities.Anuncio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<float>("Nota")
                        .HasColumnType("real");

                    b.Property<float>("Preco")
                        .HasColumnType("real");

                    b.Property<int>("ProprietarioId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ProprietarioId");

                    b.ToTable("Anuncios");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Compra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnuncioId")
                        .HasColumnType("int");

                    b.Property<int>("AssuntoId")
                        .HasColumnType("int");

                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<int>("CompradorId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AnuncioId");

                    b.HasIndex("AssuntoId");

                    b.HasIndex("AutorId");

                    b.HasIndex("CompradorId");

                    b.ToTable("Compras");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Imagem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AnuncioId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("AnuncioId");

                    b.ToTable("Imagem");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Mensagem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CompraId")
                        .HasColumnType("int");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CompraId");

                    b.ToTable("Mensagem");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<float>("Nota")
                        .HasColumnType("real");

                    b.Property<string>("Perfil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Anuncio", b =>
                {
                    b.HasOne("Balcao.Domain.Entities.Usuario", "Proprietario")
                        .WithMany()
                        .HasForeignKey("ProprietarioId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Proprietario");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Compra", b =>
                {
                    b.HasOne("Balcao.Domain.Entities.Anuncio", null)
                        .WithMany("Compras")
                        .HasForeignKey("AnuncioId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Balcao.Domain.Entities.Anuncio", "Assunto")
                        .WithMany()
                        .HasForeignKey("AssuntoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Balcao.Domain.Entities.Usuario", "Autor")
                        .WithMany()
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Balcao.Domain.Entities.Usuario", "Comprador")
                        .WithMany("Compras")
                        .HasForeignKey("CompradorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Assunto");

                    b.Navigation("Autor");

                    b.Navigation("Comprador");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Imagem", b =>
                {
                    b.HasOne("Balcao.Domain.Entities.Anuncio", null)
                        .WithMany("Imagem")
                        .HasForeignKey("AnuncioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Mensagem", b =>
                {
                    b.HasOne("Balcao.Domain.Entities.Compra", null)
                        .WithMany("Mensagens")
                        .HasForeignKey("CompraId");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Anuncio", b =>
                {
                    b.Navigation("Compras");

                    b.Navigation("Imagem");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Compra", b =>
                {
                    b.Navigation("Mensagens");
                });

            modelBuilder.Entity("Balcao.Domain.Entities.Usuario", b =>
                {
                    b.Navigation("Compras");
                });
#pragma warning restore 612, 618
        }
    }
}