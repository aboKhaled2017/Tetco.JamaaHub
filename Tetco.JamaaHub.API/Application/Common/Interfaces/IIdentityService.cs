﻿using Domain.BuildingBlocks;
using Domain.Common.Patterns;
using Domain.Entities.Identity;

namespace Application.Common.Interfaces;

public interface IIdentityService
    {
    Task<ApplicationUser> GetUserByNationalIdAsync ( string NationalId );
    Task<string?> GetUserNameAsync ( string userId );

    Task<bool> IsInRoleAsync ( string userId , string role );

    Task<bool> AuthorizeAsync ( string userId , string policyName );

    Task<(Result Result, string UserId)> CreateUserAsync ( string userName , string password );

    Task<Result> DeleteUserAsync ( string userId );
    bool IsCurrentUserInRole ( string role );
    int GetUniversityIdOfCurrentUser ( );
    }
