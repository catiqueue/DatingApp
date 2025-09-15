using API.Interfaces;

namespace API.Services;

public class SystemClockService : ISystemClockService {
  public DateTime Now => DateTime.UtcNow;
}
