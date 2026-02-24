using RAG.Application.Interfaces;
using RAG.Domain.Entities;

namespace RAG.Infrastructure.Metadata;

public class LlmMetadataGenerator : IMetadataGenerator
{
    private readonly ILLMClient _llmClient;

    public LlmMetadataGenerator(ILLMClient llmClient)
    {
        _llmClient = llmClient;
    }

    public async Task<ChunkMetadata> GenerateAsync(DocumentChunk chunk, CancellationToken cancellationToken = default)
    {
        var summaryPrompt = "Summarize the following text in one sentence:";
        var summary = await _llmClient.GenerateCompletionAsync(summaryPrompt, chunk.Content, cancellationToken);

        var keywordPrompt = "Extract up to 5 keywords from the following text. Return only the keywords, comma-separated:";
        var keywordsRaw = await _llmClient.GenerateCompletionAsync(keywordPrompt, chunk.Content, cancellationToken);

        var questionPrompt = "Generate 2-3 questions that this text can answer. Return each question on a new line:";
        var questionsRaw = await _llmClient.GenerateCompletionAsync(questionPrompt, chunk.Content, cancellationToken);

        return new ChunkMetadata
        {
            Id = Guid.NewGuid(),
            ChunkId = chunk.Id,
            Summary = summary.Trim(),
            Keywords = keywordsRaw.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList(),
            GeneratedQuestions = questionsRaw.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList(),
            HeadingContext = chunk.Metadata.GetValueOrDefault("heading")
        };
    }
}
