using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class JamaaHubForbiddenAccessException : BaseJamaaHubException
{
    public JamaaHubForbiddenAccessException() : base("Hub_Lack_Access_Error","entity has no access to this resource")
    { 
    }
}
