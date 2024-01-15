using Domain.Common.Patterns;
using Domain.Common.Exceptions;
using Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.JameahHub.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        if (result.Succeeded)
            return Result.Success();

        throw new JamaaHubInValidOperationException();
    }
}
