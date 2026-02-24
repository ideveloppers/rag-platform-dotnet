using RAG.Application.Interfaces;
using RAG.Domain.Entities;

namespace RAG.Infrastructure.Chunking;

public class StructureAwareChunkingService : IChunkingService
{
    private readonly IStructureAnalyzer _structureAnalyzer;

    public StructureAwareChunkingService(IStructureAnalyzer structureAnalyzer)
    {
        _structureAnalyzer = structureAnalyzer;
    }

    public async Task<IReadOnlyList<DocumentChunk>> ChunkAsync(string tenantId, string documentId, string content, CancellationToken cancellationToken = default)
    {
        var structure = await _structureAnalyzer.AnalyzeAsync(content, cancellationToken);
        var chunks = new List<DocumentChunk>();
        var chunkIndex = 0;

        foreach (var section in structure.Sections)
        {
            chunks.Add(new DocumentChunk
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                DocumentId = documentId,
                Content = section.Content,
                ChunkIndex = chunkIndex++,
                Metadata = new Dictionary<string, string>
                {
                    ["heading"] = section.Heading,
                    ["level"] = section.Level.ToString()
                }
            });
        }

        foreach (var table in structure.Tables)
        {
            chunks.Add(new DocumentChunk
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                DocumentId = documentId,
                Content = table.RawContent,
                ChunkIndex = chunkIndex++,
                Metadata = new Dictionary<string, string>
                {
                    ["type"] = "table",
                    ["rows"] = table.RowCount.ToString(),
                    ["columns"] = table.ColumnCount.ToString()
                }
            });
        }

        return chunks;
    }
}
