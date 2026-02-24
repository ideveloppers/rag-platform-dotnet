namespace RAG.Application.Interfaces;

public interface ILLMClient
{
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    Task<string> GenerateCompletionAsync(string prompt, string context, CancellationToken cancellationToken = default);
}
