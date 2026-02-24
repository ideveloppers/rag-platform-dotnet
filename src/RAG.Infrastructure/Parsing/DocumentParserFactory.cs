using RAG.Application.Interfaces;

namespace RAG.Infrastructure.Parsing;

public class DocumentParserFactory
{
    private readonly IEnumerable<IDocumentParser> _parsers;

    public DocumentParserFactory(IEnumerable<IDocumentParser> parsers)
    {
        _parsers = parsers;
    }

    public IDocumentParser GetParser(string fileName)
    {
        return _parsers.FirstOrDefault(p => p.CanParse(fileName))
            ?? throw new NotSupportedException($"No parser available for file: {fileName}");
    }
}
