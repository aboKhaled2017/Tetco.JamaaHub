using Abd.CleanArchitecture.Kernel.Domain;

namespace Domain.Entities.Hub;

public sealed class University : BaseEntity<int>
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
}
