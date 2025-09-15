using System.Text.Json.Serialization;

namespace API.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender {
  None, Male, Female
}
