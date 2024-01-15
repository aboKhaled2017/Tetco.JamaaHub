namespace Domain.Common.Exceptions;
public class JamaaHubInValidOperationException : BaseJamaaHubException
{
    public JamaaHubInValidOperationException(string errorMessage = "not valid business operation")
        : base("Hub_Business_Validation_Error", errorMessage)
    {
    }
}
