namespace API.Entities;

public abstract class EntityBase<TSelf> : IComparable<TSelf>, IEquatable<TSelf> where TSelf : EntityBase<TSelf> {
  public int Id { get; set; }
  public int CompareTo(TSelf? other) => Id.CompareTo(other?.Id);
  public bool Equals(TSelf? other) => Id.Equals(other?.Id);
}
