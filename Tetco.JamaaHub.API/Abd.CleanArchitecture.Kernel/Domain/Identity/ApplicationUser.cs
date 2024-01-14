using Microsoft.AspNetCore.Identity;

namespace Abd.CleanArchitecture.Kernel.Domain.Identity;

public class ApplicationUser : IdentityUser, IUser
{
    public int UniversityId { get; set; }
    public virtual ICollection<ApplicationUserGroup<Guid>> UserGroups
    {
        get; set;
    }
}
