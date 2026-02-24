namespace RAG.Domain.Enums;

public enum IngestionStatus
{
    Queued,
    Parsing,
    Chunking,
    GeneratingMetadata,
    Embedding,
    Stored,
    Failed
}
