# Ambev Developer Evaluation – Sales API

## 🛠️ Módulos & Responsabilidades

- **dentro de /backend executar**

```
docker compose up -d --build
```

```
docker cp .\scripts.sql pg-sales:/scripts.sql
```

```
docker exec -it pg-sales psql -U postgres -d DeveloperEvaluation -v ON_ERROR_STOP=1 -f /scripts.sql
```

- **acessar a API**

http://localhost:8080/swagger/index.html

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
