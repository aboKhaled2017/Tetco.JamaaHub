namespace Domain.Common.Exceptions;

public class NotValidEntityParameterException : Exception
{
    public NotValidEntityParameterException(string parameterName, string errorMessage = "is not valid")
        : base($"{parameterName}: {errorMessage}")
    {
    }
}
