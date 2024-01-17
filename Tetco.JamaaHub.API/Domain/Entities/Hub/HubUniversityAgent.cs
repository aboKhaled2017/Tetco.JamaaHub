using Abd.CleanArchitecture.Kernel.Domain;

namespace Domain.Entities.Hub;

public sealed class HubUniversityAgent : BaseEntity<int>
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string[] MackAddresses { get; set; }
    public string InstituteCode { get; set; }
}
