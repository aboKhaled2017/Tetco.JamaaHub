﻿using Abd.CleanArchitecture.Kernel.Domain;

namespace Abd.CleanArchitecture.Kernel.Domain.Identity;

public class ApplicationPermission<Guid> : BaseAuditableEntity<Guid>
   {

   public string NameAr
      {
      get; set;
      }
   public string NameEn
      {
      get; set;
      }
   public string DescriptionAr
      {
      get; set;
      }
   public string DescriptionEn
      {
      get; set;
      }
   public virtual ICollection<ApplicationRolePermission<Guid>> RolePermissions
      {
      get; set;
      }
   }


