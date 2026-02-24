using RAG.Domain.Entities;

namespace RAG.Application.Interfaces;

public interface IVectorStore
{
    Task StoreChunksAsync(IEnumerable<DocumentChunk> chunks, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DocumentChunk>> SearchAsync(string tenantId, float[] queryEmbedding, int topK = 5, CancellationToken cancellationToken = default);
    Task DeleteByDocumentIdAsync(string tenantId, string documentId, CancellationToken cancellationToken = default);
}
