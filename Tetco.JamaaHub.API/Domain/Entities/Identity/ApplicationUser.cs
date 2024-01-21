using Domain.BuildingBlocks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class ApplicationUser : IdentityUser, IUser
    {
    public int UniversityId
        {
        get; set;
        }
    public string NationalId
        {
        get; set;
        }
    public virtual ICollection<ApplicationUserGroup<Guid>> UserGroups
        {
        get; set;
        }
    }
