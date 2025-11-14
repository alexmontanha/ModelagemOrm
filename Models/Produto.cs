using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelagemOrm.Models
{
    /// <summary>
    /// Representa um produto no sistema
    /// </summary>
    [Table("produtos")] // Nome da tabela no banco
    public class Produto
    {
        [Key] // Chave primária
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
        [Column("descricao")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, 1000000, ErrorMessage = "O preço deve estar entre 0.01 e 1.000.000")]
        [Column("preco", TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo")]
        [Column("estoque")]
        public int Estoque { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Column("data_atualizacao")]
        public DateTime? DataAtualizacao { get; set; }
    }
}