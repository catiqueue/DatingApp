namespace API.Extensions;

public static class DateTimeExtensions {
  // if a person's birthday is 2006 and today is 2025,
  // the age can be 19 or 18, depending on whether they celebrate their birthday this year
  public static int GetAge(this DateOnly dob, DateOnly now) {
    int years = now.Year - dob.Year;
    var birthdayThisYear = new DateOnly(now.Year, dob.Month, dob.Day);
    return now < birthdayThisYear ? years - 1 : years;
  }
}
