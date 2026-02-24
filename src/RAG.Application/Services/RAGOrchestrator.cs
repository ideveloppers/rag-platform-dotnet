using RAG.Application.Interfaces;

namespace RAG.Application.Services;

public class RAGOrchestrator
{
    private readonly IVectorStore _vectorStore;
    private readonly ILLMClient _llmClient;

    public RAGOrchestrator(IVectorStore vectorStore, ILLMClient llmClient)
    {
        _vectorStore = vectorStore;
        _llmClient = llmClient;
    }

    public async Task<string> QueryAsync(string tenantId, string query, int topK = 5, CancellationToken cancellationToken = default)
    {
        var queryEmbedding = await _llmClient.GenerateEmbeddingAsync(query, cancellationToken);

        var relevantChunks = await _vectorStore.SearchAsync(tenantId, queryEmbedding, topK, cancellationToken);

        var context = string.Join("\n\n", relevantChunks.Select(c => c.Content));

        var response = await _llmClient.GenerateCompletionAsync(query, context, cancellationToken);

        return response;
    }
}
