using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelagemOrm.Data;
using ModelagemOrm.Models;
using ModelagemOrm.DTOs;

namespace ModelagemOrm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(AppDbContext context, ILogger<ProdutoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os produtos
        /// </summary>
        /// <returns>Lista de produtos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> GetProdutos(
            [FromQuery] bool? ativo = null,
            [FromQuery] decimal? precoMin = null,
            [FromQuery] decimal? precoMax = null)
        {
            try
            {
                var query = _context.Produtos.AsQueryable();

                // Filtros opcionais
                if (ativo.HasValue)
                    query = query.Where(p => p.Ativo == ativo.Value);

                if (precoMin.HasValue)
                    query = query.Where(p => p.Preco >= precoMin.Value);

                if (precoMax.HasValue)
                    query = query.Where(p => p.Preco <= precoMax.Value);

                var produtos = await query
                    .OrderBy(p => p.Nome)
                    .Select(p => new ProdutoResponseDto
                    {
                        Id = p.Id,
                        Nome = p.Nome,
                        Descricao = p.Descricao,
                        Preco = p.Preco,
                        Estoque = p.Estoque,
                        Ativo = p.Ativo,
                        DataCriacao = p.DataCriacao,
                        DataAtualizacao = p.DataAtualizacao
                    })
                    .ToListAsync();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar produtos");
                return StatusCode(500, "Erro interno ao buscar produtos");
            }
        }

        /// <summary>
        /// Busca um produto por ID
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>Produto encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProdutoResponseDto>> GetProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);

                if (produto == null)
                {
                    return NotFound(new { message = $"Produto com ID {id} não encontrado" });
                }

                var response = new ProdutoResponseDto
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco,
                    Estoque = produto.Estoque,
                    Ativo = produto.Ativo,
                    DataCriacao = produto.DataCriacao,
                    DataAtualizacao = produto.DataAtualizacao
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar produto {Id}", id);
                return StatusCode(500, "Erro interno ao buscar produto");
            }
        }

        /// <summary>
        /// Cria um novo produto
        /// </summary>
        /// <param name="dto">Dados do produto</param>
        /// <returns>Produto criado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProdutoResponseDto>> PostProduto(CreateProdutoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var produto = new Produto
                {
                    Nome = dto.Nome,
                    Descricao = dto.Descricao,
                    Preco = dto.Preco,
                    Estoque = dto.Estoque,
                    Ativo = dto.Ativo,
                    DataCriacao = DateTime.UtcNow
                };

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                var response = new ProdutoResponseDto
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco,
                    Estoque = produto.Estoque,
                    Ativo = produto.Ativo,
                    DataCriacao = produto.DataCriacao,
                    DataAtualizacao = produto.DataAtualizacao
                };

                return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto");
                return StatusCode(500, "Erro interno ao criar produto");
            }
        }

        /// <summary>
        /// Atualiza um produto existente
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <param name="dto">Dados atualizados</param>
        /// <returns>NoContent se sucesso</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProduto(int id, UpdateProdutoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var produto = await _context.Produtos.FindAsync(id);

                if (produto == null)
                {
                    return NotFound(new { message = $"Produto com ID {id} não encontrado" });
                }

                // Atualizar propriedades
                produto.Nome = dto.Nome;
                produto.Descricao = dto.Descricao;
                produto.Preco = dto.Preco;
                produto.Estoque = dto.Estoque;
                produto.Ativo = dto.Ativo;
                produto.DataAtualizacao = DateTime.UtcNow;

                _context.Entry(produto).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProdutoExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto {Id}", id);
                return StatusCode(500, "Erro interno ao atualizar produto");
            }
        }

        /// <summary>
        /// Remove um produto
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>NoContent se sucesso</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);

                if (produto == null)
                {
                    return NotFound(new { message = $"Produto com ID {id} não encontrado" });
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar produto {Id}", id);
                return StatusCode(500, "Erro interno ao deletar produto");
            }
        }

        /// <summary>
        /// Verifica se um produto existe
        /// </summary>
        private async Task<bool> ProdutoExists(int id)
        {
            return await _context.Produtos.AnyAsync(e => e.Id == id);
        }
    }
}