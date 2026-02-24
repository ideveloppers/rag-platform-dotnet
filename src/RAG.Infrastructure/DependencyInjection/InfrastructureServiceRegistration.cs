using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RAG.Application.Interfaces;
using RAG.Application.Orchestration;
using RAG.Infrastructure.Chunking;
using RAG.Infrastructure.LLM;
using RAG.Infrastructure.Metadata;
using RAG.Infrastructure.Parsing;
using RAG.Infrastructure.VectorStore;

namespace RAG.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.Configure<AzureOpenAISettings>(configuration.GetSection(AzureOpenAISettings.SectionName));
        services.Configure<PgVectorSettings>(configuration.GetSection(PgVectorSettings.SectionName));

        // Core services
        services.AddScoped<ILLMClient, AzureOpenAIClient>();
        services.AddScoped<IVectorStore, PgVectorStore>();

        // Data processing pipeline
        services.AddScoped<IChunkingService, StructureAwareChunkingService>();
        services.AddScoped<IMetadataGenerator, LlmMetadataGenerator>();
        services.AddScoped<DocumentParserFactory>();

        // Orchestration
        services.AddScoped<QueryPlanner>();
        services.AddScoped<RAGOrchestrator>();

        return services;
    }
}
