using System.Text.Json.Serialization;

namespace API.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserGender {
  Male, Female, Other, Unspecified
}
