# Modelagem ORM – Exemplo com ASP.NET Core + Entity Framework Core + PostgreSQL

## 📌 Visão Geral
Este repositório demonstra uma API REST básica construída em **ASP.NET Core** utilizando **Entity Framework Core (EF Core)** para acesso a dados e **PostgreSQL** como banco de dados. O objetivo é apresentar conceitos de **Modelagem de Entidades**, **Mapeamento ORM**, **Migrations**, e operações CRUD completas sobre a entidade `Produto`.

A aplicação já inclui:
- Estrutura de projeto Web API moderna (.NET 10 / EF Core 10 RC)
- Configuração do `DbContext` (`AppDbContext`)
- Entidade de domínio `Produto`
- Controller com endpoints REST (`ProdutoController`)
- Seed de dados determinístico (evitando valores dinâmicos em migrations)
- Suporte a Swagger/OpenAPI para documentação automática

> Observação: O projeto utiliza versões **RC (release candidate)** do EF Core e do provedor Npgsql para compatibilidade com .NET 10. Em ambientes de produção, recomenda-se usar versões estáveis (ex.: .NET 9 + EF Core 9 + Npgsql 9).

---
## 🧪 Tecnologias & Pacotes
| Tecnologia | Uso |
|------------|-----|
| ASP.NET Core | Estrutura da API Web |
| Entity Framework Core | ORM para acesso a dados |
| Npgsql.EntityFrameworkCore.PostgreSQL | Provedor EF para PostgreSQL |
| Swashbuckle.AspNetCore | Documentação Swagger/OpenAPI |
| .NET 10 (SDK) | Plataforma de execução |

Pacotes principais definidos em `ModelagemOrm.csproj`:
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Swashbuckle.AspNetCore`

---
## 🗂 Estrutura Simplificada
```
ModelagemOrm/
 ├── Program.cs
 ├── Data/
 │    └── AppDbContext.cs
 ├── Models/
 │    └── Produto.cs
 ├── Controllers/
 │    └── ProdutoController.cs
 ├── Migrations/ (gerada via EF Core)
 ├── appsettings.json / appsettings.Development.json
 └── README.md
```

---
## 🧩 Entidade `Produto`
Campos principais:
- `Id` (int, chave primária)
- `Nome` (string, obrigatório, indexado)
- `Descricao` (string, opcional)
- `Preco` (decimal com precisão 18,2)
- `Estoque` (int)
- `Ativo` (bool)
- `DataCriacao` (DateTime UTC)
- `DataAtualizacao` (DateTime? UTC)

Seed inicial definido em `OnModelCreating` (valores estáticos para não gerar migrations repetidas).

---
## 🌐 Endpoints Disponíveis (CRUD Produto)
Base: `/api/produto`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/produto` | Lista produtos (filtros: `ativo`, `precoMin`, `precoMax`) |
| GET | `/api/produto/{id}` | Busca produto por ID |
| POST | `/api/produto` | Cria novo produto |
| PUT | `/api/produto/{id}` | Atualiza produto existente |
| DELETE | `/api/produto/{id}` | Remove produto |

Swagger UI disponível em ambiente Development: `/swagger`.

---
## ⚙️ Configuração de Conexão
No `appsettings.json` (exemplo):
```json
"ConnectionStrings": {
  "PostgreSQLConnection": "Host=localhost;Port=5432;Database=produtos_db;Username=postgres;Password=senha"
}
```
Altere `Host`, `Database`, `Username` e `Password` conforme seu ambiente.

No `Program.cs`:
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"))
           .EnableSensitiveDataLogging()        // Apenas para desenvolvimento
           .EnableDetailedErrors());            // Apenas para desenvolvimento
```
> Remova `EnableSensitiveDataLogging()` em produção.

---
## 🚀 Como Executar
### Pré-requisitos
- .NET SDK instalado (10.x ou 9.x conforme ajuste de pacotes)
- Banco PostgreSQL acessível

### Passos
```powershell
# Restaurar dependências
dotnet restore

# (Opcional) Criar nova migration se alterou o modelo
dotnet ef migrations add NovaAlteracao

# Aplicar migrations ao banco
dotnet ef database update

# Executar a API
dotnet run
```
Acesse: `https://localhost:PORT/swagger`

### Ferramentas EF Core (se não instaladas)
```powershell
dotnet tool install --global dotnet-ef
```

---
## 🛠 Migrations
Exemplos de uso:
```powershell
# Listar migrations
dotnet ef migrations list

# Adicionar nova migration
dotnet ef migrations add AjusteProduto

# Remover última (cuidado)
dotnet ef migrations remove

# Atualizar banco
dotnet ef database update
```

Se aparecer aviso sobre modelo não determinístico, verifique se não há uso de `DateTime.UtcNow`, `Guid.NewGuid()` ou similares dentro de `HasData`.

---
## ✅ Boas Práticas Implementadas
- DTOs para separar domínio de exposições REST
- Logger para tratamento de exceções
- Filtros opcionais em listagem
- Status codes HTTP corretos (`200`, `201`, `204`, `404`, `500`)
- Determinismo em migrations (seed com datas fixas)

---
## 🔐 Observações de Segurança
- Evite expor dados sensíveis nos logs (desabilitar `EnableSensitiveDataLogging` fora de dev)
- Utilize variáveis de ambiente ou Secret Manager para credenciais de banco
- Considere validação mais rigorosa (FluentValidation) para evoluções futuras

---
## 📈 Próximos Passos (Sugestões)
- Adicionar autenticação (JWT / Identity)
- Paginação e ordenação avançada nos endpoints
- Testes unitários e de integração (xUnit)
- Versionamento da API (ex.: /api/v1)
- Cache para consultas de produtos ativos
- CI/CD (GitHub Actions) para build + migrations controladas

---
## ❓ Dúvidas & Contribuições
Fique à vontade para abrir Issues ou Pull Requests com melhorias, correções ou sugestões didáticas.

---
## 📄 Licença
Uso educacional. Defina uma licença (ex.: MIT) se desejar distribuição pública.

---
## 💬 Contato
Projeto criado para aula de Modelagem ORM. Ajuste conforme necessidades do curso ou laboratório.

---
Boas práticas, mudanças de versão ou dúvidas sobre EF Core: https://learn.microsoft.com/ef/core/

## Automation

Teste para o Lumen.us

