using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using OpenAI.Embeddings;
using RAG.Application.Interfaces;

namespace RAG.Infrastructure.LLM;

public class AzureOpenAIClient : ILLMClient
{
    private readonly Azure.AI.OpenAI.AzureOpenAIClient _client;
    private readonly AzureOpenAISettings _settings;
    private readonly ILogger<AzureOpenAIClient> _logger;

    public AzureOpenAIClient(IOptions<AzureOpenAISettings> settings, ILogger<AzureOpenAIClient> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _client = new Azure.AI.OpenAI.AzureOpenAIClient(
            new Uri(_settings.Endpoint),
            new ApiKeyCredential(_settings.ApiKey));
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Generating embedding for text of length {Length}.", text.Length);

        var embeddingClient = _client.GetEmbeddingClient(_settings.EmbeddingDeployment);

        var result = await embeddingClient.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);

        return result.Value.ToFloats().ToArray();
    }

    public async Task<string> GenerateCompletionAsync(string prompt, string context, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Generating completion for prompt of length {Length}.", prompt.Length);

        var chatClient = _client.GetChatClient(_settings.ChatDeployment);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a helpful assistant. Use the provided context to answer the user's question accurately. If the context doesn't contain relevant information, say so."),
            new UserChatMessage($"Context:\n{context}\n\nQuestion: {prompt}")
        };

        var options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = _settings.MaxTokens,
            Temperature = _settings.Temperature
        };

        var result = await chatClient.CompleteChatAsync(messages, options, cancellationToken);

        return result.Value.Content[0].Text;
    }
}
