namespace SSMS.UI.Models;

public class DbEntity {
  public enum DbEntityType {
    Table,
    View
  }

  public string Name { get; init; } = string.Empty;
  public string Schema { get; init; } = string.Empty;

  public override string ToString() => $"{Schema}.{Name}";
}