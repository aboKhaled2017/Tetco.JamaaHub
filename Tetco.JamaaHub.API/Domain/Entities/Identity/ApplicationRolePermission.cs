using Domain.BuildingBlocks;

namespace Domain.Entities.Identity;

public class ApplicationRolePermission<Guid> : BaseAuditableEntity<Guid>
   {
   public string RoleId
      {
      get; set;
      }

   public Guid? PermissionId
      {
      get; set;
      }
   public ApplicationRole Role
      {
      get; set;
      }
   public ApplicationPermission<Guid> Permission
      {
      get; set;
      }
   }