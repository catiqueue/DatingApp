using System.Diagnostics.CodeAnalysis;

namespace API.Helpers;

// "terror" is kinda funny
public record Result<TValue, TError> where TValue : notnull where TError : notnull {
  private Result() { }
  public Result(TValue value) => Value = value;
  public Result(TError error) => Error = error;
  
  [MemberNotNullWhen(true, nameof(Value))]
  [MemberNotNullWhen(false, nameof(Error))]
  public bool IsSuccess => Value is not null;
  
  [MemberNotNullWhen(true, nameof(Error))]
  [MemberNotNullWhen(false, nameof(Value))]
  public bool IsFailure => !IsSuccess;
  
  public TValue? Value { get; private set; }
  public TError? Error { get; private set; }

  public static Result<TValue, TError> Success(TValue value) => new(value);
  public static Result<TValue, TError> Failure(TError error) => new(error);
}
