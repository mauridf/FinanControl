using System;
using System.IO;
using FinanControl.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanControl.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<FonteRenda> FontesRenda { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Obtém um diretório de dados apropriado sem depender de `FileSystem`
                var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (string.IsNullOrWhiteSpace(appDataDir))
                {
                    appDataDir = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
                }

                var databasePath = Path.Combine(appDataDir, "financontrol.db");

                // Garante que o diretório exista
                var directory = Path.GetDirectoryName(databasePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                optionsBuilder.UseSqlite($"Filename={databasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.SenhaHash).IsRequired().HasMaxLength(500);
            });

            // Configurações de Conta
            modelBuilder.Entity<Conta>(entity =>
            {
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SaldoAtual).HasPrecision(18, 2);
                entity.Property(e => e.SaldoInicial).HasPrecision(18, 2);

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Contas)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurações de Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Categorias)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurações de FonteRenda
            modelBuilder.Entity<FonteRenda>(entity =>
            {
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ValorEstimado).HasPrecision(18, 2);

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.FontesRenda)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurações de Transacao
            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Valor).HasPrecision(18, 2);

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Transacoes)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Conta)
                      .WithMany(c => c.Transacoes)
                      .HasForeignKey(e => e.ContaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Categoria)
                      .WithMany(c => c.Transacoes)
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
