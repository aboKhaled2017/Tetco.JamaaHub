using Application.Common.Models;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.JameahHub.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(ErrorCodes.ApplicationError, result.Errors.Select(e => e.Description));
    }
}
