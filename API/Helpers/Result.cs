using System.Diagnostics.CodeAnalysis;

namespace API.Helpers;

// "terror" is kinda funny
public class Result<TValue, TError> where TValue : notnull where TError : notnull {
  [MemberNotNullWhen(true, nameof(Value))]
  [MemberNotNullWhen(false, nameof(Error))]
  public bool IsSuccess { get; set; }
  
  [MemberNotNullWhen(true, nameof(Error))]
  [MemberNotNullWhen(false, nameof(Value))]
  public bool IsFailure => !IsSuccess;
  
  public TValue? Value { get; set; }
  public TError? Error { get; set; }
  
  public static Result<TValue, TError> Success(TValue value) => new() { IsSuccess = true, Value = value };
  public static Result<TValue, TError> Failure(TError error) => new() { IsSuccess = false, Error = error };
}
