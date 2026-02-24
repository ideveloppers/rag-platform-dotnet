using RAG.Domain.Enums;

namespace RAG.Domain.Entities;

public class DataSource
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public DataSourceType SourceType { get; set; }
    public IngestionStatus Status { get; set; } = IngestionStatus.Queued;
    public long FileSizeBytes { get; set; }
    public string? ContentHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}
