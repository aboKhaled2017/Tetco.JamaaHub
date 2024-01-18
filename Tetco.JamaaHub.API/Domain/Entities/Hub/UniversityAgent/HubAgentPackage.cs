using Abd.CleanArchitecture.Kernel.Domain;
using Domain.Entities.Hub.UniversityAgent.ValueObjects;

namespace Domain.Entities.Hub.UniversityAgent;

/// <summary>
/// maintain the information about the received package of the  daily  batch received from Agent
/// </summary>
public sealed class HubAgentPackage : BaseEntity<long>
{
    private HubAgentPackage()
    {
    }

    /// <summary>
    /// to which batch has been assigned
    /// </summary>
    public HubAgentBatch HubAgentBatch { get; set; }
    public long HubAgentBatchId { get; set; }

    /// <summary>
    /// the actual package row data fetched from the Agent
    /// this data will be analysed later , and saved to normalized data according the defined schema
    /// </summary>
    public string PackageData { get; set; }

    /// <summary>
    /// addtional information about te package
    /// </summary>
    public PackageMetaData MetaData { get; set; }

    /// <summary>
    /// check if the package has invalid records after the analysis operation being completed
    /// this will be considered only in case of the package has been already completed analysis
    /// </summary>
    public bool HasInValidrecords { get; set; }

    /// <summary>
    /// how many invalid records remained after the analysis operation being completed
    /// this will be considered only in case of the package has been already completed analysis
    /// </summary>
    public int NumofInvalidrecords { get; set; }

    public bool IsAnalysisCompleted()
        => MetaData.IsAnalysed && MetaData.CompletedAnalysisAt.HasValue;

    public bool IsSynCompleted()
        => MetaData.IsSynced && MetaData.CompletedSyncingAt.HasValue;


    public static HubAgentPackage Create(HubAgentBatch batch,string data,DateTime agentSentAt)
    {
        return new HubAgentPackage
        {
            HubAgentBatch = batch,
            HubAgentBatchId= batch.Id,
            PackageData = data,
            MetaData=new(agentSentAt,DateTime.Now)
        };
    }
}
