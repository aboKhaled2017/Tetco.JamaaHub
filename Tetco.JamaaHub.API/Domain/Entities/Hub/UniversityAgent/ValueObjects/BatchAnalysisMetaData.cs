namespace Domain.Entities.Hub.UniversityAgent.ValueObjects
{
    /// <summary>
    /// this object maintains a state about the patch analysis information on the received packages
    /// </summary>
    public sealed record BatchAnalysisMetaData
    {
 
        /// <summary>
        /// how many packages are synced sofar
        /// </summary>
        public int? NumOfAnalysedPackages { get; set; }

        /// <summary>
        /// status of analysing operation
        /// </summary>
        public BatchAnalysisStatus Status { get; set; } = BatchAnalysisStatus.New;

        /// <summary>
        /// when the analysis operation has started on th received packages?
        /// </summary>
        public DateTime? startAt { get; set; }

        /// <summary>
        /// when the analysis operation has completed with landing zone
        /// </summary>
        public DateTime? ComplatedAt { get; set; }
    }
}
