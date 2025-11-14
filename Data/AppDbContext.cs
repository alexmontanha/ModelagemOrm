using Microsoft.EntityFrameworkCore;
using ModelagemOrm.Models;

namespace ModelagemOrm.Data
{
    /// <summary>
    /// Contexto do banco de dados - Gerencia as entidades e conexão com PostgreSQL
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet representa uma tabela no banco de dados
        public DbSet<Produto> Produtos { get; set; }

        /// <summary>
        /// Configurações adicionais do modelo
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações da entidade Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                // Índice para busca por nome
                entity.HasIndex(p => p.Nome);

                // Configurar precisão do decimal
                entity.Property(p => p.Preco)
                    .HasPrecision(18, 2);

                // Seed data (dados iniciais)
                entity.HasData(
                    new Produto
                    {
                        Id = 1,
                        Nome = "Notebook Dell",
                        Descricao = "Notebook para desenvolvimento",
                        Preco = 3500.00M,
                        Estoque = 10,
                        Ativo = true,
                        DataCriacao = new DateTime(2025, 11, 14, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new Produto
                    {
                        Id = 2,
                        Nome = "Mouse Logitech",
                        Descricao = "Mouse ergonômico sem fio",
                        Preco = 150.00M,
                        Estoque = 50,
                        Ativo = true,
                        DataCriacao = new DateTime(2025, 11, 14, 0, 0, 0, DateTimeKind.Utc)
                    }
                );
            });
        }
    }
}