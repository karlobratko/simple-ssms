namespace SSMS.UI.Models;
public sealed record class TabPageTag(int Id, string FileName) {
  public static TabPageTag Empty() => new(0, $"SQLQuery{0}.sql");
}
