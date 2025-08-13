namespace API.Services;

public interface ISystemClockService { 
  DateTime Now { get; }
}

public class SystemClockService : ISystemClockService {
  public DateTime Now => DateTime.UtcNow;
}
