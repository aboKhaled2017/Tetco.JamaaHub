using Domain.BuildingBlocks;

namespace Domain.Entities.Hub.UniversityAgent;

/// <summary>
/// represents the Agent entity
/// </summary>
public sealed class HubAgent : BaseEntity<int>
{
    public HubAgent()
    {
        SchemaTypes = new();
    }
    public string NameAr { get; set; }
    public string NameEn { get; set; }

    /// <summary>
    /// unique code to identify the Agent university
    /// </summary>
    public string InstituteCode { get; set; }
    public string IpAddress { get; set; }

    /// <summary>
    /// the base url of the hosted agent service that hub will comunicate with to fetch the batches
    /// </summary>
    public string AgentServiceUrl { get; set; }

    /// <summary>
    /// the access key of the agent hosted service , it's required for the hub to be authorized for doing operations with Agent
    /// </summary>
    public string AgentApiAccessKey { get; set; }

    /// <summary>
    /// how many Schema types are assigned for Agent
    /// </summary>
    public List<HubAgentSchema> SchemaTypes { get; set; }
}
