using Domain.BuildingBlocks;

namespace Domain.Entities.Hub.UniversityAgent;

/// <summary>
/// information about the schema that assigned to Agent
/// </summary>
public sealed class HubAgentSchema:BaseEntity<string>
{
    private HubAgentSchema()
    {
        
    }
    private HubAgentSchema(HubAgent hubAgent, string id, string name, string version)
    {
        HubAgent=hubAgent;
        Id=id;
        Name=name;
        Version=version;
    }
    public string Version { get; set; }
    public string Name { get; set; }
    public int HubAgentId { get; set; }
    public HubAgent HubAgent { get; set; }

    /// <summary>
    /// factory method to create an Agent schema
    /// </summary>
    /// <param name="hubAgent">Agent to assigne the schema to</param>
    /// <param name="id">schema identifier</param>
    /// <param name="name">scehma name</param>
    /// <param name="version">schema version</param>
    /// <returns></returns>
    public static HubAgentSchema Create(HubAgent hubAgent, string id, string name, string version)
        => new(hubAgent, id, name, version);
}
