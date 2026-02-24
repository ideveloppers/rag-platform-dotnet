namespace RAG.Application.Validation;

public interface IAuditor
{
    Task<AuditResult> AuditAsync(string tenantId, string query, string response, CancellationToken cancellationToken = default);
}

public class AuditResult
{
    public bool IsCompliant { get; set; }
    public List<string> Violations { get; set; } = new();
    public string? Notes { get; set; }
}
