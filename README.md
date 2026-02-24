#   RAG Platform (.NET 8 + Azure OpenAI)

Enterprise-grade **Agentic RAG (Retrieval-Augmented Generation)**
platform built with:

-   .NET 8
-   Clean Architecture
-   PostgreSQL + pgvector
-   Azure OpenAI
-   Multi-tenant ready
-   Production-oriented design

------------------------------------------------------------------------

##   Overview

This project implements a modular, production-ready RAG architecture
designed for:

-   Enterprise AI copilots
-   Knowledge assistants
-   AI-enabled SaaS platforms
-   Regulated domains (finance, legal, compliance)

It supports:

-   Multi-agent orchestration
-   Vector search
-   Tenant isolation
-   Evaluation logging
-   Cost tracking
-   Scalable cloud deployment

------------------------------------------------------------------------

##   Architecture

### High-Level Flow

Client → API (ASP.NET Core) → RAG Orchestrator → Planner → Agents →
Vector Store → Azure OpenAI

------------------------------------------------------------------------

### Solution Structure

    rag-platform-dotnet/
    ├── src/
    │   ├── RAG.Api/
    │   ├── RAG.Application/
    │   ├── RAG.Domain/
    │   ├── RAG.Infrastructure/
    │   ├── RAG.Workers/
    ├── tests/
    │   ├── RAG.UnitTests/
    │   ├── RAG.IntegrationTests/
    ├── docker/
    ├── scripts/
    ├── .github/workflows/
    └── rag-platform.sln

------------------------------------------------------------------------

##   Local Development

### Start PostgreSQL (Docker)

``` bash
cd docker
docker-compose up -d
```

### Run API

``` bash
dotnet run --project src/RAG.Api
```

------------------------------------------------------------------------

##   License

MIT
