namespace API.Extensions;

public static class DateTimeExtensions {
  // I wanted to make this a one-liner, but it's not very readable 😭😭😭
  public static uint GetAge(this DateOnly dob, DateOnly now) 
    => (uint) (now.Year - dob.Year - (now < new DateOnly(now.Year, dob.Month, dob.Day) ? 1 : 0));
}
