using API.Services.Abstractions;

namespace API.Services;

public class SystemClockService : ISystemClockService {
  public DateTime Now => DateTime.UtcNow;
}
