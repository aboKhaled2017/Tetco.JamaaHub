using Domain.BuildingBlocks;

namespace Domain.Entities.Identity;

public class ApplicationUserGroup<Guid> : BaseAuditableEntity<Guid>
   {
   public string UserId
      {
      get; set;
      }

   public Guid? GroupId
      {
      get; set;
      }
   public ApplicationUser User
      {
      get; set;
      }
   public ApplicationGroup<Guid> Group
      {
      get; set;
      }
   }
