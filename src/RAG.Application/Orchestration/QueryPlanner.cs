using RAG.Domain.Enums;

namespace RAG.Application.Orchestration;

public class QueryPlanner
{
    public QueryRoute Plan(string query, string tenantId)
    {
        return new QueryRoute
        {
            RouteType = QueryRouteType.DirectRetrieval,
            TopK = 5
        };
    }
}

public class QueryRoute
{
    public QueryRouteType RouteType { get; set; }
    public int TopK { get; set; } = 5;
    public string? TargetAgentName { get; set; }
    public bool RequiresHumanValidation { get; set; }
}
