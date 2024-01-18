namespace Domain.Entities.Hub.UniversityAgent.ValueObjects
{
    /// <summary>
    /// this object maintains a state about the patch syncing information
    /// </summary>
    public sealed record BatchSyncMetaData
    {
        /// <summary>
        /// this is the number of valid packages going to be saynced with landing zone for specific batch , only these packages will be synced to landing zone
        /// </summary>
        public int? NumOfValidPackages { get; set; }

        /// <summary>
        /// how many packages are synced sofar
        /// </summary>
        public int? NumOfSyncedPackages { get; set; }

        /// <summary>
        /// status of syncing operation
        /// </summary>
        public BatchSyncStatus Status { get; set; } = BatchSyncStatus.New;

        /// <summary>
        /// when the syncing operation has started?
        /// </summary>
        public DateTime? startAt { get; set; }

        /// <summary>
        /// when the syncing operation has completed with landing zone
        /// </summary>
        public DateTime? ComplatedAt { get; set; }
    }
}
