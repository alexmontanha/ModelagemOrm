using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ModelagemOrm.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "produtos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    preco = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    estoque = table.Column<int>(type: "integer", nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produtos", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "produtos",
                columns: new[] { "id", "ativo", "data_atualizacao", "data_criacao", "descricao", "estoque", "nome", "preco" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Notebook para desenvolvimento", 10, "Notebook Dell", 3500.00m },
                    { 2, true, null, new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Mouse ergonômico sem fio", 50, "Mouse Logitech", 150.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_produtos_nome",
                table: "produtos",
                column: "nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produtos");
        }
    }
}
