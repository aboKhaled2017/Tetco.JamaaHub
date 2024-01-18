using Abd.CleanArchitecture.Kernel.Domain;

namespace Domain.Entities.Hub.UniversityAgent;

/// <summary>
/// information about the schema that assigned to Agent
/// </summary>
public sealed class HubAgentSchema:BaseEntity<string>
{
    public string Version { get; set; }
    public string Name { get; set; }
    public int HubAgentId { get; set; }
    public HubAgent HubAgent { get; set; }
}
