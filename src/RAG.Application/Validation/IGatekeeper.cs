using RAG.Domain.Enums;

namespace RAG.Application.Validation;

public interface IGatekeeper
{
    Task<ValidationDecision> ReviewAsync(string tenantId, string query, string proposedResponse, CancellationToken cancellationToken = default);
}

public class ValidationDecision
{
    public ValidationStatus Status { get; set; }
    public string? Reason { get; set; }
}
