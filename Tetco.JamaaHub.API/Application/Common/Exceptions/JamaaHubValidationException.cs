using Domain.Common.Exceptions;
using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class JamaaHubValidationException : BaseJamaaHubException
{
    public JamaaHubValidationException()
        : base("Hub_Not_Valid_Request_Error", "One or more validation failures have occurred.")
    {

    }

    public JamaaHubValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}
