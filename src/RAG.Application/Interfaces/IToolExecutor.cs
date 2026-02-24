namespace RAG.Application.Interfaces;

public interface IToolExecutor
{
    string ToolName { get; }
    Task<string> ExecuteAsync(string input, CancellationToken cancellationToken = default);
}
