namespace RAG.Application.Interfaces;

public interface IStructureAnalyzer
{
    Task<DocumentStructure> AnalyzeAsync(string rawContent, CancellationToken cancellationToken = default);
}

public class DocumentStructure
{
    public List<DocumentSection> Sections { get; set; } = new();
    public List<TableBlock> Tables { get; set; } = new();
    public List<string> Headings { get; set; } = new();
}

public class DocumentSection
{
    public string Heading { get; set; } = default!;
    public string Content { get; set; } = default!;
    public int Level { get; set; }
}

public class TableBlock
{
    public string RawContent { get; set; } = default!;
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
}
