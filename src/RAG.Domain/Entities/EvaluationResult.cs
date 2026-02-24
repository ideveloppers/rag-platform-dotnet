namespace RAG.Domain.Entities;

public class EvaluationResult
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = default!;
    public string Query { get; set; } = default!;
    public string Response { get; set; } = default!;
    public double? PrecisionScore { get; set; }
    public double? RecallScore { get; set; }
    public double? LlmJudgeScore { get; set; }
    public long LatencyMs { get; set; }
    public decimal? EstimatedCost { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
