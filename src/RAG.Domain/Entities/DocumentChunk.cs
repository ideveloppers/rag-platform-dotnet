namespace RAG.Domain.Entities;

public class DocumentChunk
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = default!;
    public string DocumentId { get; set; } = default!;
    public string Content { get; set; } = default!;
    public float[] Embedding { get; set; } = Array.Empty<float>();
    public int ChunkIndex { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
