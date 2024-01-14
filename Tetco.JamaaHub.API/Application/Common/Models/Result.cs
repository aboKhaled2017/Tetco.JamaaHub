using Domain.Constants;

namespace Application.Common.Models;

public class Result : Result<object>
{
    public Result() // only for serializing
    {

    }
    internal Result(bool succeeded, IEnumerable<string> errors, string message = null) : base(succeeded, errors, message)
    {
    }

    public static Result Success(string message = null)
    {
        return new Result(true, Array.Empty<string>())
        {
            Message = message
        };
    }
    public static Result Failure(string errorCode, string errorMessage)
    {
        return new Result(false, Enumerable.Empty<string>())
        {
            ErrorCode = errorCode,
            Message = errorMessage
        };
    }
    public static Result Failure(string errorCode, IEnumerable<string> errors)
    {
        return new Result(false, errors)
        {
            ErrorCode = errorCode
        };
    }
}
public class Result<TData>
{
    public Result() // only for serializing
    {

    }
    internal Result(bool succeeded, IEnumerable<string> errors, string message = null)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Message = message;
    }

    public bool Succeeded
    {
        get; init;
    }
    public string[] Errors
    {
        get; init;
    }
    public string Message { get; set; }
    public string ErrorCode { get; set; }

    public static Result<TData> Success(string message = null)
    {
        return new Result<TData>(true, Array.Empty<string>())
        {
            Message = message
        };
    }
    public static Result<TData> Failure(string errorCode, string errorMessage)
    {
        return new Result<TData>(false, Enumerable.Empty<string>())
        {
            ErrorCode = errorCode,
            Message = errorMessage
        };
    }
    public static Result<TData> Failure(string errorCode, IEnumerable<string> errors)
    {
        return new Result<TData>(false, errors)
        {
            ErrorCode = errorCode
        };
    }
    public TData Data { get; set; }
    public Result<TData> WithData(TData data)
    {
        Data = data;
        return this;
    }
}
