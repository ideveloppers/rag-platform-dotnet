namespace RAG.Infrastructure.VectorStore;

public class PgVectorSettings
{
    public const string SectionName = "PgVector";

    public string ConnectionString { get; set; } = default!;
    public string TableName { get; set; } = "document_chunks";
    public int EmbeddingDimensions { get; set; } = 1536;
}
