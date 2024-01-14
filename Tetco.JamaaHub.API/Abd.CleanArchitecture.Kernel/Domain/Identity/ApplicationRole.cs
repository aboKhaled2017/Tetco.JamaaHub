﻿using Microsoft.AspNetCore.Identity;

namespace Abd.CleanArchitecture.Kernel.Domain.Identity;

public class ApplicationRole : IdentityRole
   {
   public virtual ICollection<ApplicationRolePermission<Guid>> RolePermissions
      {
      get; set;
      }

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
   }
