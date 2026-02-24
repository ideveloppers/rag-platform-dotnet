namespace RAG.Application.Interfaces;

public interface IAgent
{
    string Name { get; }
    Task<string> ExecuteAsync(string input, string tenantId, CancellationToken cancellationToken = default);
}
