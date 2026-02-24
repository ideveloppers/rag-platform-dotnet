using RAG.Application.Interfaces;
using RAG.Domain.Enums;

namespace RAG.Application.Orchestration;

public class RAGOrchestrator
{
    private readonly IVectorStore _vectorStore;
    private readonly ILLMClient _llmClient;
    private readonly QueryPlanner _queryPlanner;
    private readonly IEnumerable<IAgent> _agents;

    public RAGOrchestrator(
        IVectorStore vectorStore,
        ILLMClient llmClient,
        QueryPlanner queryPlanner,
        IEnumerable<IAgent> agents)
    {
        _vectorStore = vectorStore;
        _llmClient = llmClient;
        _queryPlanner = queryPlanner;
        _agents = agents;
    }

    public async Task<string> QueryAsync(string tenantId, string query, CancellationToken cancellationToken = default)
    {
        var route = _queryPlanner.Plan(query, tenantId);

        return route.RouteType switch
        {
            QueryRouteType.DirectRetrieval => await HandleDirectRetrievalAsync(tenantId, query, route.TopK, cancellationToken),
            QueryRouteType.AgentPipeline => await HandleAgentPipelineAsync(tenantId, query, route.TargetAgentName!, cancellationToken),
            _ => await HandleDirectRetrievalAsync(tenantId, query, route.TopK, cancellationToken)
        };
    }

    private async Task<string> HandleDirectRetrievalAsync(string tenantId, string query, int topK, CancellationToken cancellationToken)
    {
        var queryEmbedding = await _llmClient.GenerateEmbeddingAsync(query, cancellationToken);
        var relevantChunks = await _vectorStore.SearchAsync(tenantId, queryEmbedding, topK, cancellationToken);
        var context = string.Join("\n\n", relevantChunks.Select(c => c.Content));
        return await _llmClient.GenerateCompletionAsync(query, context, cancellationToken);
    }

    private async Task<string> HandleAgentPipelineAsync(string tenantId, string query, string agentName, CancellationToken cancellationToken)
    {
        var agent = _agents.FirstOrDefault(a => a.Name == agentName)
            ?? throw new InvalidOperationException($"Agent '{agentName}' not found.");

        return await agent.ExecuteAsync(query, tenantId, cancellationToken);
    }
}
