using Domain.Enums;

namespace Domain.Entities.Hub.UniversityAgent.ValueObjects
{
    /// <summary>
    /// express the status of analysis information of the batch
    /// </summary>
    public sealed record BatchAnalysisStatus : SmartEnum<BatchAnalysisStatus>
    {
        public BatchAnalysisStatus(string key, string value) : base(key, value)
        {
        }

        public static BatchAnalysisStatus New = new("new", "the batch hasn't started analysing packages");
        public static BatchAnalysisStatus InProgress = new("in-progress", "the batch has started analyisin packagesg");
        public static BatchAnalysisStatus Completed = new("completed", "the batch has comleted analyising packages");
    }
}
