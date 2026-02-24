using RAG.Application.Interfaces;

namespace RAG.Workers.Ingestion;

public class EmbeddingWorker : BackgroundService
{
    private readonly ILogger<EmbeddingWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public EmbeddingWorker(ILogger<EmbeddingWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Embedding worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var llmClient = scope.ServiceProvider.GetRequiredService<ILLMClient>();
                var vectorStore = scope.ServiceProvider.GetRequiredService<IVectorStore>();

                // TODO: Pick up chunks without embeddings, generate and store them

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in embedding worker.");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        _logger.LogInformation("Embedding worker stopped.");
    }
}
