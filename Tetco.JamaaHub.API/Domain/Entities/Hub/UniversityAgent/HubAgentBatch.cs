using Abd.CleanArchitecture.Kernel.Domain;
using Domain.Entities.Hub.UniversityAgent.ValueObjects;

namespace Domain.Entities.Hub.UniversityAgent;

/// <summary>
/// maintain the information about the daily batch received from Agent
/// </summary>
public sealed class HubAgentBatch : BaseAuditableEntity<long>
{
    private HubAgentBatch()
    {
        SyncMetaData = new();
        AnalysisMetaData= new();
    }
    public int HubAgentId { get; set; }
    public HubAgent HubAgent { get; set; }

    public string HubAgentSchemaId { get; set; }
    public HubAgentSchema HubAgentSchema { get; set; }

    public BatchTransferMetaData TransferMetaData { get; set; }
    public BatchSyncMetaData SyncMetaData { get; set; }
    public BatchAnalysisMetaData AnalysisMetaData { get; set; }

    /// <summary>
    /// factory method to intiate new package
    /// </summary>
    /// <param name="agent">the Agent from whom the batch will be received</param>
    /// <param name="schemaId">teh schema type for which the packages will be fetched</param>
    /// <param name="totalExpectedPackages">how many packages are expected to be received?</param>
    /// <param name="totalExpectedrecords">how many records are expected to be received from Agent</param>
    /// <returns></returns>
    public static HubAgentBatch Create(HubAgent agent,string schemaId, int totalExpectedPackages, int totalExpectedrecords)
    {
        return new HubAgentBatch
        {
            HubAgent = agent,
            HubAgentId=agent.Id,
            HubAgentSchemaId=schemaId,
            TransferMetaData=new(totalExpectedPackages, totalExpectedrecords, 0,DateTime.Now)
        };
    }
}
