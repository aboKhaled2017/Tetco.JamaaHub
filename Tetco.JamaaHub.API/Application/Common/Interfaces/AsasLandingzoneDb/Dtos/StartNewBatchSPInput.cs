namespace Application.Common.Interfaces.AsasLandingzoneDb.Dtos
{
    public sealed record StartNewBatchSPInput (
        int SchemaTypeId,int MigrationTypeId,
        Guid BatchId, string SchemaVersion,
        int TotalRecordsCount, DateTime StartDate,
        int PriorityLevelId,string InstituteCode)
    {
    }

    public sealed record StopCurrentBatchSPInput(
       string SchemaTypeId, int MigrationTypeId,
       Guid BatchId, string InstituteCode,DateTime endDate)
    {
    }

    public sealed record InsertNewPackageSpInput()
    {

    }
}
