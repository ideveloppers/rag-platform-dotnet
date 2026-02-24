using RAG.Domain.Entities;

namespace RAG.Application.Interfaces;

public interface IChunkingService
{
    Task<IReadOnlyList<DocumentChunk>> ChunkAsync(string tenantId, string documentId, string content, CancellationToken cancellationToken = default);
}
