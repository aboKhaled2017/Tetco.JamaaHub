using Domain.Enums;

namespace Domain.Entities.Hub.UniversityAgent.ValueObjects
{
    /// <summary>
    /// express the status of syncing information of the batch
    /// </summary>
    public sealed record BatchSyncStatus : SmartEnum<BatchSyncStatus>
    {
        public BatchSyncStatus(string key, string value) : base(key, value)
        {
        }

        public static BatchSyncStatus New = new("new", "the batch hasn't started syncing to landing zone yet");
        public static BatchSyncStatus InProgress = new("in-progress", "the batch has started syncing with landing zone");
        public static BatchSyncStatus Completed = new("completed", "the batch has been synced completely to landing zone");
    }

    /// <summary>
    /// maintains information about the Agent package
    /// </summary>
    /// <param name="AgentSentAt">when did aget sent the package to the hub</param>
    /// <param name="HubrRceivedAt">when did the hub received teh package</param>
    public sealed record PackageMetaData (DateTime AgentSentAt, DateTime HubrRceivedAt)
    {
        public bool IsAnalysed { get; set; }
        public DateTime? StartedAnalysisAt { get; set; }
        public DateTime? CompletedAnalysisAt { get; set; }


        public bool IsSynced { get; set; }
        public DateTime? StartedSyncingAt { get; set; }
        public DateTime? CompletedSyncingAt { get; set; }

    }
}
