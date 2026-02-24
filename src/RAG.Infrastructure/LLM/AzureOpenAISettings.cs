namespace RAG.Infrastructure.LLM;

public class AzureOpenAISettings
{
    public const string SectionName = "AzureOpenAI";

    public string Endpoint { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
    public string EmbeddingDeployment { get; set; } = "text-embedding-ada-002";
    public string ChatDeployment { get; set; } = "gpt-4o";
    public int EmbeddingDimensions { get; set; } = 1536;
    public int MaxTokens { get; set; } = 2048;
    public float Temperature { get; set; } = 0.3f;
}
