# Ambev Developer Evaluation – Sales API
> **Escopo deste README:** somente a **estrutura do projeto** (pastas, camadas, responsabilidades e artefatos). As instruções de execução e Docker serão adicionadas depois.

---

## 🌳 Estrutura do Repositório

/
├─ src/
│ ├─ Ambev.DeveloperEvaluation.WebApi/
│ │ ├─ Controllers/
│ │ │ ├─ SalesController.cs
│ │ │ └─ AuthController.cs
│ │ ├─ Auth/
│ │ │ ├─ JwtOptions.cs
│ │ │ └─ JwtTokenService.cs
│ │ ├─ Filters/ # Filtros globais (ex: exception handling)
│ │ ├─ Extensions/ # Extensões de IServiceCollection / IApplicationBuilder
│ │ ├─ Middlewares/ # Middlewares customizados (se houver)
│ │ ├─ Mappings/ # Profiles do AutoMapper específicos da WebApi (opcional)
│ │ ├─ Program.cs
│ │ ├─ appsettings.json
│ │ └─ appsettings.Development.json
│ │
│ ├─ Ambev.DeveloperEvaluation.Application/
│ │ ├─ Contracts/
│ │ │ ├─ Requests/ # DTOs de entrada (ex: CreateSaleRequest)
│ │ │ └─ Responses/ # DTOs de saída (ex: SaleResponse, PagedResult)
│ │ ├─ Interfaces/
│ │ │ ├─ Services/ # ISaleService, IAuthService, etc.
│ │ │ └─ Repositories/ # ISaleRepository, IUserRepository (ports)
│ │ ├─ Services/ # Implementações de casos de uso (Application Service Layer)
│ │ ├─ Handlers/ # (Opcional) MediatR Handlers (Commands/Queries)
│ │ ├─ Commands/ # (Opcional) Commands do domínio de Sales
│ │ ├─ Queries/ # (Opcional) Queries do domínio de Sales
│ │ ├─ Validators/ # FluentValidation validators (ex: CreateSaleValidator)
│ │ └─ Mapping/ # Profiles do AutoMapper (camada de app)
│ │
│ ├─ Ambev.DeveloperEvaluation.Domain/
│ │ ├─ Common/
│ │ │ ├─ BaseEntity.cs # Id, DomainEvents etc.
│ │ │ └─ EntityExtensions.cs # Helpers de domínio (se houver)
│ │ ├─ Entities/
│ │ │ ├─ Sale.cs
│ │ │ └─ SaleItem.cs
│ │ ├─ Enums/
│ │ │ └─ SaleStatus.cs
│ │ ├─ Events/
│ │ │ ├─ SaleCreatedEvent.cs
│ │ │ ├─ SaleModifiedEvent.cs
│ │ │ └─ SaleCancelledEvent.cs
│ │ └─ Specifications/ # (Opcional) especificações de consulta
│ │
│ ├─ Ambev.DeveloperEvaluation.Infrastructure/
│ │ ├─ Persistence/
│ │ │ ├─ Context/
│ │ │ │ └─ DefaultContext.cs # DbContext EF Core
│ │ │ ├─ Configurations/ # IEntityTypeConfiguration<>, mapeamentos EF
│ │ │ └─ Migrations/ # Migrações geradas pelo EF Core
│ │ ├─ Repositories/
│ │ │ ├─ SaleRepository.cs
│ │ │ └─ UserRepository.cs
│ │ ├─ Identity/ # (Opcional) implementação de usuários/roles
│ │ ├─ Messaging/ # (Opcional) simulação de publicação de eventos (ILogger)
│ │ └─ Logging/ # Configurações de Serilog (se houver)
│ │
│ └─ Ambev.DeveloperEvaluation.ORM/
│ ├─ DesignTime/
│ │ └─ DefaultContextFactory.cs # Factory para migrações
│ └─ Migrations/ # (se as migrações ficarem isoladas neste projeto)
│
├─ tests/
│ ├─ Ambev.DeveloperEvaluation.Unit/
│ │ ├─ Domain/
│ │ │ └─ Entities/
│ │ │ ├─ SaleItemTests.cs
│ │ │ └─ SaleTests.cs
│ │ ├─ Domain/Entities/Shared/
│ │ │ ├─ SaleItemFaker.cs
│ │ │ └─ SaleFaker.cs
│ │ ├─ Application/ # (Opcional) testes de services/handlers
│ │ └─ Auth/ # (Opcional) testes de AuthService/JWT
│ │
│ ├─ Ambev.DeveloperEvaluation.Integration/
│ │ └─ SalesEndpointTests.cs # Exemplo de integração (WebApplicationFactory)
│ │
│ └─ Ambev.DeveloperEvaluation.Functional/
│ └─ AuthAndSalesFlowTests.cs # Fluxos ponta-a-ponta simplificados (com Skip até Docker)
│
├─ scripts/
│ ├─ seed.sql # Inserts iniciais (Users, Sales, SaleItems)
│ ├─ dump.sql # Dump do schema/dados (opcional)
│ └─ postman_collection.json # Coleção para testes manuais (opcional)
│
├─ docker/
│ ├─ Dockerfile # (a criar)
│ └─ docker-compose.yml # (a criar: api + db)
│
├─ docs/
│ └─ architecture.md # Diagrama de camadas, decisões de design, trade-offs
│
├─ .editorconfig
├─ .gitignore
├─ .gitattributes
├─ Ambev.DeveloperEvaluation.sln
└─ README.md

## 🧩 Módulos & Responsabilidades

- **WebApi**
  - Exposição HTTP (controllers/DTOs).
  - Autenticação/autorização (JWT).
  - Swagger/OpenAPI.
  - Middlewares/filtros globais.
- **Application**
  - Regras de aplicação e orquestração de casos de uso.
  - DTOs (Requests/Responses), validações (FluentValidation).
  - Mapeamentos (AutoMapper).
  - Interfaces (ports) para repositórios/serviços externos.
- **Domain**
  - Entidades ricas (`Sale`, `SaleItem`), regras de negócio e eventos de domínio.
  - Enums e abstrações (`BaseEntity`, domain events).
- **Infrastructure**
  - Implementação de repositórios (EF Core).
  - `DbContext`, configurações de mapeamento e migrações.
  - Integrações técnicas (logging, identity, mensageria simulada).
- **ORM** (opcional)
  - Isolar fábrica de contexto e migrações (design-time).
- **Tests**
  - **Unit:** regras de negócio e casos de uso (Bogus + FluentAssertions + NSubstitute).
  - **Integration:** testes com `WebApplicationFactory`.
  - **Functional:** fluxos ponta-a-ponta simplificados.

---

## 📚 Padrões e Bibliotecas (previstas)
- **DDD leve** (Domain, Application, Infrastructure, WebApi)
- **AutoMapper** (mapeamento DTO ↔ entidades)
- **FluentValidation** (validação)
- **MediatR** (opcional: comandos/queries/handlers)
- **Serilog** (logging)
- **xUnit**, **FluentAssertions**, **Bogus**, **NSubstitute** (testes)

---

## 🗃️ Artefatos de dados
- `scripts/seed.sql` → inserts mínimos (Users, Sales, SaleItems) para popular ambiente.
- `scripts/dump.sql` → snapshot opcional para reconstituir base.
- `docs/architecture.md` → visão técnica do sistema e decisões (ex.: regras de desconto, eventos de domínio, paginação/ordenção/filtros).
