namespace RAG.Application.Interfaces;

public interface IDocumentParser
{
    Task<string> ParseAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    bool CanParse(string fileName);
}
