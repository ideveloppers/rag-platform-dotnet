using RAG.Domain.Entities;

namespace RAG.Application.Evaluation;

public interface IEvaluator
{
    Task<EvaluationResult> EvaluateAsync(string tenantId, string query, string response, IReadOnlyList<DocumentChunk> retrievedChunks, long latencyMs, CancellationToken cancellationToken = default);
}
