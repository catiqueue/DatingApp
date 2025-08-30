using System.ComponentModel.DataAnnotations;

namespace API.Helpers;

internal class StringLengthRangeAttribute : StringLengthAttribute {
  public StringLengthRangeAttribute(int minimumLength, int maximumLength) : base(maximumLength) {
    MinimumLength = minimumLength;
  }
}
