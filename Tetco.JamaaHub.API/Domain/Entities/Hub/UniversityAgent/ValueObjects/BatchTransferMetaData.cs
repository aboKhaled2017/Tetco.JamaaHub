namespace Domain.Entities.Hub.UniversityAgent.ValueObjects
{
    /// <summary>
    /// this class contains a state about teh batch tranfer information
    /// </summary>
    /// <param name="NumOfTotalPackages">what is the total packges expected to be received from Agent</param>
    /// <param name="NumOfTotalRecords">what is the total records expected to be received from the aggregate total Agent packages</param>
    /// <param name="NumOfTransferedPackages">how many packages have been received from Agent so far</param>
    /// <param name="startAt">when the batch ahs satrted transfering packages from Agent</param>
    public sealed record BatchTransferMetaData(
        int NumOfTotalPackages,
        int NumOfTotalRecords,
        int NumOfTransferedPackages,
        DateTime startAt)
    {
        /// <summary>
        /// the transfer status
        /// </summary>
        public BatchTransferStatus Status { get; set; }= BatchTransferStatus.New;

        /// <summary>
        /// when the batch has completed transfering packages from Agent
        /// </summary>
        public DateTime? ComplatedAt { get; set; }
    }
}
