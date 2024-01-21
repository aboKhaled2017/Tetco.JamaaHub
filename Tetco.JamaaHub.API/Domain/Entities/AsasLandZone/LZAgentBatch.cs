using Domain.BuildingBlocks;

namespace Domain.Entities.AsasLandZone;

public sealed class LZAgentBatch : BaseEntity<long>
{
    public int SchemaTypeId { get; set; }
    public LZSchemaType SchemaType { get; set; }
    public int MigrationTypeId { get; set; }
    public string InstituteCode { get; set; }
    public Guid BatchGUID { get; set; }
    public string SchemaVersion { get; set; }
    public long TotalRecordsCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsBatchCompleted { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public int? PriorityLevelId { get; set; }
}
