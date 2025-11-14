using System.ComponentModel.DataAnnotations;

namespace ModelagemOrm.DTOs
{
    /// <summary>
    /// DTO para criação de produto
    /// </summary>
    public class CreateProdutoDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, 1000000)]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue)]
        public int Estoque { get; set; }

        public bool Ativo { get; set; } = true;
    }

    /// <summary>
    /// DTO para atualização de produto
    /// </summary>
    public class UpdateProdutoDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue)]
        public int Estoque { get; set; }

        public bool Ativo { get; set; }
    }

    /// <summary>
    /// DTO para resposta de produto
    /// </summary>
    public class ProdutoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}