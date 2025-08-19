namespace API.Services.Abstractions;

public interface ISystemClockService { 
  DateTime Now { get; }
}
