using Domain.Common.Patterns;

namespace Domain.Common.Exceptions;

public abstract class BaseJamaaHubException: Exception
{
    public BaseJamaaHubException(string code,string errorMessage):base(errorMessage)
    {
        Code = code;
    }
    public IDictionary<string, string[]> Errors { get; set; }
    public string Code { get; set; }
    
    public TException WithError<TException>(string key, IEnumerable<string> errors)
        where TException : BaseJamaaHubException

    {
        if(!Errors.ContainsKey(key))
          Errors.Add(key, errors.ToArray());
      
        return this as TException;
    }

    public Result ToResultError()
    {
        return Result.Failure(Code, Errors, Message);
    }
}
