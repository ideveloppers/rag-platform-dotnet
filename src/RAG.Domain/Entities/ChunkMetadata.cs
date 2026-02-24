namespace RAG.Domain.Entities;

public class ChunkMetadata
{
    public Guid Id { get; set; }
    public Guid ChunkId { get; set; }
    public string? Summary { get; set; }
    public List<string> Keywords { get; set; } = new();
    public List<string> GeneratedQuestions { get; set; } = new();
    public string? HeadingContext { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
