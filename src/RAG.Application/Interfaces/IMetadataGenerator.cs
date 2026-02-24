using RAG.Domain.Entities;

namespace RAG.Application.Interfaces;

public interface IMetadataGenerator
{
    Task<ChunkMetadata> GenerateAsync(DocumentChunk chunk, CancellationToken cancellationToken = default);
}
